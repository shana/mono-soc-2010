using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Decompiler.Ast {
	public class ExprAdd : ExprBinaryOpArithmetic {

		public ExprAdd (MethodInfo methodInfo, Expr left, Expr right, Sn signage, bool overflow)
			: base (methodInfo, left, right, signage, overflow)
		{
		}

		public override ExprType ExprType
		{
			get { return ExprType.Add; }
		}

	}
}
