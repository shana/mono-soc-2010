using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Decompiler;
using Decompiler.Ast;
using System.Diagnostics.Contracts;
using Mono.Cecil;
using Decompiler.Visitors;

namespace ccrewrite {
	class TransformContractsVisitor : ExprVisitor {

		public TransformContractsVisitor (MethodDefinition method)
		{
			this.module = method.Module;
			this.methodInfo = new MethodInfo (method);
		}

		private ModuleDefinition module;
		private MethodInfo methodInfo;
		private List<Tuple<Expr, Expr>> toReplace = new List<Tuple<Expr, Expr>> ();

		public IEnumerable<Tuple<Expr, Expr>> ToReplace
		{
			get
			{
				return this.toReplace;
			}
		}

		protected override Expr VisitCall (ExprCall e)
		{
			var call = (ExprCall)base.VisitCall (e);

			var method = e.Method;
			if (method.DeclaringType.FullName == "System.Diagnostics.Contracts.Contract") {
				switch (method.Name) {
				case "Requires":
					if (!method.HasGenericParameters) {
						switch (method.Parameters.Count) {
						case 1:
							return this.ProcessRequires1 (call);
						case 2:
							return this.ProcessRequires2 (call);
						default:
							throw new NotSupportedException ("Invalid number of parameters to Contract.Requires()");
						}
					} else {
						goto default;
					}
				default:
					throw new NotSupportedException ("Cannot handle Contract." + e.Method.Name + "()");
				}
			}

			return call;
		}

		private Expr ProcessRequires1 (ExprCall e)
		{
			//Expr conditionExpr = e.Parameters.First ();

			//var mWriteLine2 = typeof (Console).GetMethod ("WriteLine", new [] { typeof (string), typeof (object) });
			//var m = module.Import (mWriteLine2);
			//var loadStringExpr = new ExprLoadString (this.methodInfo, "Condition evaluates to: {0}");
			//var arg1Expr = new ExprBox (this.methodInfo, conditionExpr);
			//var call = new ExprCall (this.methodInfo, m, new Expr[] { loadStringExpr, arg1Expr });

			//this.toReplace.Add (Tuple.Create<Expr, Expr> (e, call));

			//return call;

			MethodDefinition mRequires = ContractsRuntime.GetRequires ();
			Expr conditionExpr = e.Parameters.First ();
			Expr nullArgExpr = new ExprLoadConstant (this.methodInfo, null);
			Expr conditionStringExpr = new ExprLoadString (this.methodInfo, "<Cannot find condition expression>");
			var call = new ExprCall (this.methodInfo, mRequires, new Expr [] { conditionExpr, nullArgExpr, conditionStringExpr });

			this.toReplace.Add (Tuple.Create<Expr, Expr> (e, call));

			return call;
		}

		private Expr ProcessRequires2 (ExprCall e)
		{
			throw new NotImplementedException ();
		}

	}
}
