using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cccheck.Visitors;
using Mono.Cecil;
using cccheck.Ast;

namespace cccheck {

	class FindMethodCallsVisitor : ExprVisitor {

		private List<MethodReference> calls = new List<MethodReference> ();

		public IEnumerable<MethodReference> Calls
		{
			get { return this.calls; }
		}

		protected override Expr VisitCall (ExprCall e)
		{
			this.calls.Add (e.Method);
			return base.VisitCall (e);
		}

	}

}
