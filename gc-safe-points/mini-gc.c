/*
 * mini-gc.c: GC interface for the mono JIT
 *
 * Author:
 *   Zoltan Varga (vargaz@gmail.com)
 *   Sanjoy Das (sanjoy@playingwithpointers.com)
 *
 * Copyright 2009 Novell, Inc (http://www.novell.com)
 */

#include <mono/metadata/gc-internal.h>
#include <mono/mini/mini-gc.h>

#ifdef HAVE_SGEN_GC

#include <mono/metadata/sgen-archdep.h>
#include <mono/metadata/sgen-gc.h>


#ifdef MONO_GC_SAFE_POINTS

typedef struct {
	/*
	 * The various instructions that were patched and their locations.
	 */
	GPtrArray *previous_instructions; 
	GPtrArray *safe_point_ips;

	gpointer sigctx;
	int thread_state;
} TlsJitInfo;

enum {
	THREAD_RUNNING,
	THREAD_PARKED_AT_SAFE_POINT,
	THREAD_PARKED_FORCED
};

typedef gboolean (*ForEachStackFrameFunc) (MonoJitInfo * /* Jit info */, MonoContext *, int /* counter */, gpointer /* user_data */);

static void
for_each_stack_frame (gpointer sigctx, ForEachStackFrameFunc func, gpointer user_data)
{
	MonoDomain *domain = mono_domain_get ();
	MonoJitTlsData *jit_tls = TlsGetValue (mono_get_jit_tls_key ());
	MonoLMF *lmf = mono_get_lmf ();
	MonoContext ctx, new_ctx;
	MonoJitInfo *ji, rji;

	int counter = 0;
	MonoMethod *last_method = NULL;
	mono_arch_sigctx_to_monoctx (sigctx, &ctx);

	g_assert (MONO_CONTEXT_GET_IP (&ctx) == ARCH_SIGCTX_IP (sigctx));

	while (1) {
		ji = mono_find_jit_info (domain, jit_tls, &rji, NULL, &ctx, &new_ctx, NULL, &lmf, NULL, NULL);
		if (!ji || ji == (gpointer)-1 || MONO_CONTEXT_GET_SP (&ctx) >= jit_tls->end_of_stack)
			break; 
		
		/* 
		 * See mini-exceptions.c : ves_icall_get_frame_info for an explanation
		 */
		if (ji->method->wrapper_type == MONO_WRAPPER_MANAGED_TO_NATIVE && ji->method == last_method)
			continue;
		last_method = ji->method;

		if (!func (ji, &ctx, counter, user_data))
			break;
		ctx = new_ctx;
		counter++;
	}
}

static gpointer
thread_attach (void)
{
	return g_malloc0 (sizeof (TlsJitInfo));
}

static void
int_3_handler (int sig, siginfo_t *siginfo, void *context)
{
	mono_arch_patch_safe_point_context (context);
	mono_sgen_thread_ready_to_suspend (context);
}

static void
gc_exception_callback (gpointer sigctx)
{
	mono_sgen_thread_ready_to_suspend (sigctx);
}

static struct sigaction prev_sigaction_info;

static void
thread_collector_initiate_parking (void)
{
	struct sigaction sinfo;
	sigfillset (&sinfo.sa_mask);
	sinfo.sa_flags = SA_RESTART | SA_SIGINFO;
	sinfo.sa_sigaction = int_3_handler;
	if (sigaction (SIGTRAP, &sinfo, &prev_sigaction_info) != 0)
		g_error ("failed sigaction to set int_3_handler");
	mono_arch_install_exception_callback (gc_exception_callback);
}

static void
thread_collector_dissolve_parking (void)
{
	if (sigaction (SIGTRAP, &prev_sigaction_info, NULL) != 0)
		g_error ("failed sigaction to unset int_3_handler");
	mono_arch_install_exception_callback (NULL);
}

static gboolean
iterate_find_safe_point_ip (MonoJitInfo *ji, MonoContext *ctx, int counter, gpointer user_data)
{
	GPtrArray *ips = user_data;
	MonoGCInfo *gc_info = ji->gc_info;
	MonoSafePointJitInfo *safe_points = gc_info->safe_points;
	gpointer native_ip = MONO_CONTEXT_GET_IP (ctx);
	int i;

	/*
	 * Checking mono_sgen_is_inconsistent_method is better than imposing some arbitrary restriction
	 * on managed allocators and write barriers.
	 *
	 * Backward jump safe points are not 'counted in' when the method is inconsistent.
	 */
	gboolean inconsistent = mono_sgen_is_inconsistent_method (ji->method);

	if (native_ip < ji->code_start)
		return TRUE;

	for (i = 0; i < gc_info->safe_points_len; i++) {
		if (safe_points [i].reason == SAFE_POINT_TYPE_RETURN) {
			g_ptr_array_add (ips, (gpointer) (safe_points [i].native_offset + ji->code_start));
			DEBUG_SAFE_POINTS ("%s ", mono_jit_safe_point_descr (&(safe_points [i])));
		} else if (!inconsistent && safe_points [i].reason == SAFE_POINT_TYPE_BACKWARD_BRANCH) {
			if ((safe_points [i].native_offset + ji->code_start) >= native_ip) {
				g_ptr_array_add (ips, (gpointer) (safe_points [i].native_offset + ji->code_start));
				DEBUG_SAFE_POINTS ("%s ", mono_jit_safe_point_descr (&(safe_points [i])));
			}
		} else if (safe_points [i].reason == SAFE_POINT_TYPE_METHOD_CALL) {
			g_ptr_array_add (ips, (gpointer) (safe_points [i].native_offset + ji->code_start));
			DEBUG_SAFE_POINTS ("%s ", mono_jit_safe_point_descr (&(safe_points [i])));
		}
	}

	DEBUG_SAFE_POINTS ("\n");

	return FALSE;
}

static void
thread_mutator_suspend (gpointer user_data, gpointer context)
{
	MonoDomain *domain = mono_domain_get ();
	TlsJitInfo *tls = user_data;
	GPtrArray *ips = g_ptr_array_new ();

	tls->thread_state = THREAD_RUNNING;
	tls->sigctx = context;

	/*
	 * HACK!
	 *
	 * No clue when or why this is NULL. It *is* NULL sometimes, and
	 * when it is, mono gets a SIGSEGV when calling mono_jit_info_table_find.
	 */
	if (!mono_thread_internal_current ()) {
		g_warning ("mono_thread_internal_current () returned NULL");
		tls->thread_state = THREAD_PARKED_FORCED;
		return;
	}

	if (!mono_jit_info_table_find (domain, ARCH_SIGCTX_IP (context))) {
		tls->thread_state = THREAD_PARKED_FORCED;
		return;
	}

	for_each_stack_frame (context, iterate_find_safe_point_ip, ips);

	tls->safe_point_ips = ips;
}

static void
thread_collector_suspend (gpointer user_data)
{
	TlsJitInfo *tls = user_data;
	int i;

	if (tls->thread_state == THREAD_PARKED_FORCED)
		return;

	tls->previous_instructions = g_ptr_array_new ();

	DEBUG_SAFE_POINTS ("Patching %d instructions.\n", ip_data.ips->len);
	for (i = 0; i < tls->safe_point_ips->len; i++)
		g_ptr_array_add (tls->previous_instructions, mono_arch_emit_safe_point (g_ptr_array_index (tls->safe_point_ips, i)));

	tls->thread_state = THREAD_PARKED_AT_SAFE_POINT;

	DEBUG_SAFE_POINTS ("Patched thread %p.\n", (gpointer) pthread_self ());
}

static void
thread_mutator_suspend_now (gpointer user_data)
{
	TlsJitInfo *tls = user_data;

	if (tls->thread_state == THREAD_PARKED_FORCED)
		mono_sgen_thread_ready_to_suspend (tls->sigctx);
}

static void
thread_collector_restart (gpointer user_data)
{
	TlsJitInfo *tls = user_data;

	if (tls->thread_state == THREAD_RUNNING)
		g_warning ("thread_resume_prepare_func called with tls->thread_state == THREAD_RUNNING");

	if (tls->thread_state == THREAD_PARKED_AT_SAFE_POINT) {
		int i;
		for (i = 0; i < tls->safe_point_ips->len; i++) 
			mono_arch_clear_safe_point (g_ptr_array_index (tls->safe_point_ips, i), g_ptr_array_index (tls->previous_instructions, i));
		g_ptr_array_free (tls->safe_point_ips, TRUE);
		g_ptr_array_free (tls->previous_instructions, TRUE);
	}

	tls->thread_state = THREAD_RUNNING;
	tls->sigctx = NULL;
}

static void
thread_mutator_restart (gpointer user_data)
{
	/* 
	 * Not needed now, kept around with the hope that this will be required later.
	 * Will remove it if not.
	 */
}

static void
thread_detach (gpointer user_data)
{
	g_assert (user_data);
	g_free (user_data);
}

#endif /* MONO_GC_SAFE_POINTS */

void
mini_gc_init (void)
{
	MonoGCCallbacks cb;

	memset (&cb, 0, sizeof (cb));
#ifdef MONO_GC_SAFE_POINTS
	cb.thread_attach_func = thread_attach;
	cb.thread_collector_initiate_parking_func = thread_collector_initiate_parking;
	cb.thread_mutator_suspend_func = thread_mutator_suspend;
	cb.thread_collector_suspend_func = thread_collector_suspend;
	cb.thread_mutator_suspend_now = thread_mutator_suspend_now;
	cb.thread_collector_dissolve_parking_func = thread_collector_dissolve_parking;
	cb.thread_collector_restart_func = thread_collector_restart;
	cb.thread_mutator_restart_func = thread_mutator_restart;
	cb.thread_detach_func = thread_detach;
#endif
	mono_gc_set_gc_callbacks (&cb);
}

#else

void
mini_gc_init (void)
{
}

#endif /* HAVE_SGEN_GC */

/*
 * mini_gc_init_cfg:
 *
 *   Set GC specific options in CFG.
 */
void
mini_gc_init_cfg (MonoCompile *cfg)
{
	if (mono_gc_is_moving ()) {
		cfg->disable_ref_noref_stack_slot_share = TRUE;
		cfg->gen_write_barriers = TRUE;
	}
}

