using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Decompiler;
using Decompiler.Visitors;
using Mono.Cecil.Cil;
using Decompiler.Ast;

namespace ccrewrite {
	class Rewriter {

		public Rewriter (ISymbolWriter sym)
		{
			this.sym = sym;
		}

		private ISymbolWriter sym;
		private Dictionary<MethodDefinition, TransformContractsVisitor> rewrittenMethods = new Dictionary<MethodDefinition, TransformContractsVisitor> ();

		public void Rewrite (AssemblyDefinition assembly)
		{
			var allMethods =
				from module in assembly.Modules
				from type in module.Types
				from method in type.Methods
				select method;

			foreach (var m in allMethods.ToArray ()) {
				//if (m.Name != "Test") continue;
				Console.WriteLine (m.FullName);
				this.PerformRewrite (m);
			}

		}

		private void PerformRewrite (MethodDefinition method)
		{
			if (this.rewrittenMethods.ContainsKey (method)) {
				return;
			}
			var overridden = this.GetOverriddenMethod (method);
			if (overridden != null) {
				this.PerformRewrite (overridden);
			}
			var baseMethod = this.GetBaseOverriddenMethod (method);
			if (baseMethod != method) {
				// Contract inheritance must be used
				var vOverriddenTransform = this.rewrittenMethods [baseMethod];
				// Can be null if overriding an abstract method
				if (vOverriddenTransform != null) {
					if (CmdOptions.Level >= 2) {
						// Only insert re-written contracts if level >= 2
						foreach (var inheritedRequires in vOverriddenTransform.ContractRequiresInfo) {
							this.RewriteIL (method.Body, null, null, inheritedRequires.RewrittenExpr);
						}
					}
				}
			}

			TransformContractsVisitor vTransform = null;
			if (method.HasBody) {
				vTransform = this.TransformContracts (method);
				this.sym.Write (method.Body);
			}
			this.rewrittenMethods.Add (method, vTransform);
		}

		private TransformContractsVisitor TransformContracts (MethodDefinition method)
		{
			var body = method.Body;
			Decompile decompile = new Decompile (method);
			var decomp = decompile.Go ();

			TransformContractsVisitor vTransform = new TransformContractsVisitor (method, decompile.Instructions);
			var transformed = vTransform.Visit (decomp);

			var il = body.GetILProcessor ();

			foreach (var replacement in vTransform.ContractRequiresInfo) {
				// Only insert re-written contracts if level >= 2
				Expr rewritten = CmdOptions.Level >= 2 ? replacement.RewrittenExpr : null;
				this.RewriteIL (body, decompile.Instructions, replacement.OriginalExpr, rewritten);
			}

			return vTransform;
		}

		private void RewriteIL (MethodBody body, Dictionary<Expr,Instruction> instructionLookup, Expr remove, Expr insert)
		{
			var il = body.GetILProcessor ();
			Instruction instInsertBefore;
			if (remove != null) {
				var vInstExtent = new InstructionExtentVisitor (instructionLookup);
				vInstExtent.Visit (remove);
				instInsertBefore = vInstExtent.Instructions.Last ().Next;
				foreach (var instRemove in vInstExtent.Instructions) {
					il.Remove (instRemove);
				}
			} else {
				instInsertBefore = body.Instructions [0];
			}
			if (insert != null) {
				var compiler = new CompileVisitor (il, instructionLookup, inst => il.InsertBefore (instInsertBefore, inst));
				compiler.Visit (insert);
			}
		}

		private MethodDefinition GetOverriddenMethod (MethodDefinition method)
		{
			if (method.IsNewSlot || !method.IsVirtual) {
				return null;
			}
			var baseType = method.DeclaringType.BaseType;
			if (baseType == null) {
				return null;
			}
			var overridden = baseType.Resolve ().Methods.FirstOrDefault (x => x.Name == method.Name);
			return overridden;
		}

		private MethodDefinition GetBaseOverriddenMethod (MethodDefinition method)
		{
			var overridden = method;
			for (; ; ) {
				var overriddenTemp = this.GetOverriddenMethod (overridden);
				if (overriddenTemp == null) {
					return overridden;
				}
				overridden = overriddenTemp;
			}
		}

	}
}
