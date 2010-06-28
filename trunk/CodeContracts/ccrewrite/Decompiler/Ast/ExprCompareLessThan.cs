using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Decompiler.Ast {
	public class ExprCompareLessThan : ExprBinaryOpComparison {

		public ExprCompareLessThan (MethodInfo methodInfo, Expr left, Expr right, Sn signage)
			: base (methodInfo, left, right, signage)
		{
		}

		public override ExprType ExprType
		{
			get { return ExprType.CompareLessThan; }
		}

	}
}
