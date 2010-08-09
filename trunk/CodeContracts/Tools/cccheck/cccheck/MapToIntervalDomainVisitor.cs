using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cccheck.Visitors;
using cccheck.Ast;

namespace cccheck {
	class MapToIntervalDomainVisitor : ExprVisitor {

		public MapToIntervalDomainVisitor (Environment env)
		{
			this.env = env;
		}

		private Environment env;

		public IntervalDomain Value { get; private set; }

		protected override Expr VisitLoadConstant (ExprLoadConstant e)
		{
			if (!(e.Value is int)) {
				throw new NotSupportedException ();
			}

			int v = (int) e.Value;
			this.Value = new IntervalDomain<int> (v, v);

			return e;
		}

		protected override Expr VisitLoadLocal (ExprLoadLocal e)
		{
			var value = this.env.Get (e.Variable);
			this.Value = value;
			return e;
		}

		protected override Expr VisitLoadArg (ExprLoadArg e)
		{
			var value = this.env.Get (e.Parameter);
			this.Value = value;
			return e;
		}

		protected override Expr VisitAdd (ExprAdd e)
		{
			this.Visit (e.Left);
			var leftValue = this.Value;
			this.Visit (e.Right);
			var rightValue = this.Value;
			this.Value = leftValue.Add (rightValue);
			return e;
		}

		protected override Expr VisitCompareEqual (ExprCompareEqual e)
		{
			this.Visit (e.Left);
			var leftValue = this.Value;
			this.Visit (e.Right);
			var rightValue = this.Value;
			IntervalDomain<bool> result;
			if (leftValue.IsSingleValue && rightValue.IsSingleValue) {
				bool same = leftValue.AObj.Equals (rightValue.AObj);
				result = new IntervalDomain<bool> (same);
			} else if (leftValue.DoesIntersect (rightValue)) {
				result = IntervalDomain<bool>.Top;
			} else {
				result = new IntervalDomain<bool> (false);
			}
			this.Value = result;

			return e;
		}

		protected override Expr VisitCompareGreaterThan (ExprCompareGreaterThan e)
		{
			this.Visit (e.Left);
			var leftValue = this.Value;
			this.Visit (e.Right);
			var rightValue = this.Value;
			IntervalDomain<bool> result;
			var l = (IntervalDomain<int>) leftValue;
			var r = (IntervalDomain<int>) rightValue;
			if (l.A > r.B) {
				result = new IntervalDomain<bool> (true);
			} else if (l.B <= r.A) {
				result = new IntervalDomain<bool> (false);
			} else {
				result = IntervalDomain<bool>.Top;
			}
			this.Value = result;

			return e;
		}

		protected override Expr VisitLoadLength (ExprLoadLength e)
		{
			this.Value = new IntervalDomain<int> (0, int.MaxValue-1000);
			return e;
		}

	}
}
