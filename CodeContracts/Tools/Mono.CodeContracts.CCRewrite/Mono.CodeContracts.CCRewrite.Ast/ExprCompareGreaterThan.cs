using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mono.CodeContracts.CCRewrite.Ast {
	class ExprCompareGreaterThan : ExprBinaryOpComparison {

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
