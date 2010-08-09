using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cccheck.Decompiler;

namespace cccheck.Ast {
	public class ExprCompareGreaterThan : ExprBinaryOpComparison {

		public ExprCompareGreaterThan (MethodInfo methodInfo, Expr left, Expr right, Sn signage)
			: base (methodInfo, left, right, signage)
		{
		}

		public override ExprType ExprType
		{
			get { return ExprType.CompareGreaterThan; }
		}

	}
}
