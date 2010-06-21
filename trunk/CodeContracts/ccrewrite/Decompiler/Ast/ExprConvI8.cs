using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Decompiler.Ast {
	public class ExprConvI8 : Expr {

		public ExprConvI8 (MethodInfo methodInfo, Expr exprToConvert)
			: base (methodInfo, ExprType.ConvI8, methodInfo.TypeInt64)
		{
			this.ExprToConvert = exprToConvert;
		}

		public Expr ExprToConvert { get; private set; }

	}
}
