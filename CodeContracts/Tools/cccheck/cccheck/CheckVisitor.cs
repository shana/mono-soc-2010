using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cccheck.Visitors;
using cccheck.Ast;

namespace cccheck {

	class CheckVisitor : ExprVisitor {

		public CheckVisitor (Environment initialEnv)
		{
			this.initialEnv = initialEnv;
			this.envs = new Dictionary<Expr, Environment> ();
			this.Calls = Enumerable.Empty<IntervalMethodCall> ();
		}

		private Environment initialEnv;
		private Dictionary<Expr, Environment> envs;

		private Environment curEnv;

		public IEnumerable<IntervalMethodCall> Calls { get; private set; }

		protected override Expr VisitBlock (ExprBlock e)
		{
			Environment prevEnv = this.initialEnv;
			foreach (Expr expr in e.Exprs) {
				this.curEnv = prevEnv;
				this.Visit (expr);
				this.envs [expr] = this.curEnv;
				prevEnv = this.curEnv;
			}
			return e;
		}

		protected override Expr VisitStoreLocal (ExprStoreLocal e)
		{
			MapToIntervalDomainVisitor v = new MapToIntervalDomainVisitor(this.curEnv);
			v.Visit(e.Value);
			IntervalDomain<int> value = (IntervalDomain<int>)v.Value;
			this.curEnv = this.curEnv.Set (e.Variable, value);
			return e;
		}

		protected override Expr VisitCall (ExprCall e)
		{
			if (e.Method.DeclaringType.FullName == "System.Diagnostics.Contracts.Contract") {
				this.CheckContract (e);
			} else {
				List<IntervalDomain<int>> parameters = new List<IntervalDomain<int>> ();
				foreach (var parameter in e.Parameters) {
					MapToIntervalDomainVisitor v = new MapToIntervalDomainVisitor (this.curEnv);
					v.Visit (parameter);
					IntervalDomain<int> value = (IntervalDomain<int>)v.Value;
					parameters.Add (value);
				}
				IntervalMethodCall call = new IntervalMethodCall (e, parameters);
				this.Calls = this.Calls.Concat (new [] { call }).ToArray ();
			}
			return e;
		}

		private void CheckContract (ExprCall e)
		{
			switch (e.Method.Name) {
			case "Requires":
				this.CheckRequires (e);
				break;
			default:
				throw new NotSupportedException ("Cannot handle: " + e.Method.Name);
			}
		}

		private void CheckRequires (ExprCall e)
		{
			MapToIntervalDomainVisitor v = new MapToIntervalDomainVisitor (this.curEnv);
			var p0 = e.Parameters.First ();
			v.Visit (p0);
			var value = (IntervalDomain<bool>) v.Value;
			if (value.IsSingleValue && value.A) {
				Console.WriteLine ("Contract ok");
			} else {
				Console.WriteLine ("Contract failed");
			}
		}

	}

}
