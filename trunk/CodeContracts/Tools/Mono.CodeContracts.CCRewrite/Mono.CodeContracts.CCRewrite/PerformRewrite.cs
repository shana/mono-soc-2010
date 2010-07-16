﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.CodeContracts.CCRewrite.Ast;
using Mono.CodeContracts.CCRewrite.AstVisitors;

namespace Mono.CodeContracts.CCRewrite {
	class PerformRewrite {

		public PerformRewrite (ISymbolWriter sym, RewriterOptions options)
		{
			this.sym = sym;
			this.options = options;
		}

		private ISymbolWriter sym;
		private RewriterOptions options;
		private Dictionary<MethodDefinition, TransformContractsVisitor> rewrittenMethods = new Dictionary<MethodDefinition, TransformContractsVisitor> ();

		public void Rewrite (AssemblyDefinition assembly)
		{

			foreach (var module in assembly.Modules) {
				ContractsRuntime contractsRuntime = new ContractsRuntime(module, this.options);

				var allMethods =
					from type in module.Types
					from method in type.Methods
					select method;

				foreach (var method in allMethods.ToArray ()) {
					//Console.WriteLine (method.FullName);
					this.RewriteMethod (method, contractsRuntime);
				}
			}
		}

		private void RewriteMethod (MethodDefinition method, ContractsRuntime contractsRuntime)
		{
			if (this.rewrittenMethods.ContainsKey (method)) {
				return;
			}
			var overridden = this.GetOverriddenMethod (method);
			if (overridden != null) {
				this.RewriteMethod (overridden, contractsRuntime);
			}
			bool anyRewrites = false;
			var baseMethod = this.GetBaseOverriddenMethod (method);
			if (baseMethod != method) {
				// Contract inheritance must be used
				var vOverriddenTransform = this.rewrittenMethods [baseMethod];
				// Can be null if overriding an abstract method
				if (vOverriddenTransform != null) {
					if (this.options.Level >= 2) {
						// Only insert re-written contracts if level >= 2
						foreach (var inheritedRequires in vOverriddenTransform.ContractRequiresInfo) {
							this.RewriteIL (method.Body, null, null, inheritedRequires.RewrittenExpr);
							anyRewrites = true;
						}
					}
				}
			}

			TransformContractsVisitor vTransform = null;
			if (method.HasBody) {
				vTransform = this.TransformContracts (method, contractsRuntime);
				if (this.sym != null) {
					this.sym.Write (method.Body);
				}
				if (vTransform.ContractRequiresInfo.Any ()) {
					anyRewrites = true;
				}
			}
			this.rewrittenMethods.Add (method, vTransform);

			if (anyRewrites) {
				Console.WriteLine (method.FullName);
			}
		}

		private TransformContractsVisitor TransformContracts (MethodDefinition method, ContractsRuntime contractsRuntime)
		{
			var body = method.Body;
			Decompile decompile = new Decompile (method);
			var decomp = decompile.Go ();

			TransformContractsVisitor vTransform = new TransformContractsVisitor (method, decompile.Instructions, contractsRuntime);
			var transformed = vTransform.Visit (decomp);

			var il = body.GetILProcessor ();

			foreach (var replacement in vTransform.ContractRequiresInfo) {
				// Only insert re-written contracts if level >= 2
				Expr rewritten = this.options.Level >= 2 ? replacement.RewrittenExpr : null;
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
