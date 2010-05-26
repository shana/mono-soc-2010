//
// Mono.VisualC.Interop.ABI.VTable.cs: abstract vtable
//
// Author:
//   Alexander Corrado (alexander.corrado@gmail.com)
//
// Copyright (C) 2010 Alexander Corrado
//

using System;
using System.Collections.Generic;

using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace Mono.VisualC.Interop.ABI {
        public abstract class VTable : IDisposable {
                protected IntPtr basePtr, vtPtr;

                public virtual int EntryCount { get; protected set; }

                public abstract int EntrySize { get; }
                public abstract void EmitVirtualCall (ILGenerator il, IntPtr native, int index);

                // Creates a new VTable
                public VTable (Delegate[] overrides)
                {
                        EntryCount = overrides.Length;

                        int vtableSize = EntryCount * EntrySize;
                        IntPtr vtEntryPtr;

                        basePtr = IntPtr.Zero;
                        vtPtr = Marshal.AllocHGlobal (vtableSize);

                        try {
                                int offset = 0;
                                for (int i = 0; i < EntryCount; i++) {

                                        if (overrides [i] != null) // managed override
                                                vtEntryPtr = Marshal.GetFunctionPointerForDelegate (overrides [i]);
                                        else
                                                vtEntryPtr = IntPtr.Zero;

                                        Marshal.WriteIntPtr (vtPtr, offset, vtEntryPtr);
                                        offset += EntrySize;
                                }
                        } catch {

                                Marshal.FreeHGlobal (vtPtr);
                                throw;
                        }
                }

                public virtual void InitInstance (IntPtr instance)
                {
                        if (basePtr == IntPtr.Zero) {
                                basePtr = Marshal.ReadIntPtr (instance);

                                int offset = 0;
                                for (int i = 0; i < EntryCount; i++) {

                                        IntPtr vtEntryPtr = Marshal.ReadIntPtr (vtPtr, offset);
                                        if (vtEntryPtr == IntPtr.Zero)
                                                Marshal.WriteIntPtr (vtPtr, offset, Marshal.ReadIntPtr (basePtr, offset));

                                        offset += EntrySize;
                                }
                        }

                        Marshal.WriteIntPtr (instance, vtPtr);
                }

                public virtual void ResetInstance (IntPtr instance)
                {
                        Marshal.WriteIntPtr (instance, basePtr);
                }

                public IntPtr Pointer {
                        get { return vtPtr; }
                }

                protected virtual void Dispose (bool disposing)
                {
                        if (vtPtr != IntPtr.Zero) {
                                Marshal.FreeHGlobal (vtPtr);
                                vtPtr = IntPtr.Zero;
                        }
                }

                // TODO: This WON'T usually be called because VTables are associated with classes
                //  and managed C++ class wrappers are staticly held?
                public void Dispose ()
                {
                        Dispose (true);
                        GC.SuppressFinalize (this);
                }

                ~VTable ()
                {
                        Dispose (false);
                }

                public static bool BindOverridesOnly (MemberInfo member, object obj)
                {
                        bool result = BindAny (member, obj);
                        if (member.GetCustomAttributes (typeof (OverrideNativeAttribute), true).Length != 1)
                                return false;

                        return result;
                }

                public static bool BindAny (MemberInfo member, object obj)
                {
                        MethodInfo imethod = (MethodInfo) obj;
                        MethodInfo candidate = (MethodInfo) member;

                        if (!candidate.Name.Equals (imethod.Name))
                                return false;

                        ParameterInfo[] invokeParams = imethod.GetParameters ();
                        ParameterInfo[] methodParams = candidate.GetParameters ();

                        if (invokeParams.Length == methodParams.Length) {
                                for (int i = 0; i < invokeParams.Length; i++) {
                                        if (!invokeParams [i].ParameterType.IsAssignableFrom (methodParams [i].ParameterType))
                                                return false;
                                }
                        } else if (invokeParams.Length == methodParams.Length + 1) {
                                for (int i = 1; i < invokeParams.Length; i++) {
                                        if (!invokeParams [i].ParameterType.IsAssignableFrom (methodParams [i - 1].ParameterType))
                                                return false;
                                }
                        } else
                                return false;

                        return true;
                }
        }
}
