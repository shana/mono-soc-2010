using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Decompiler.Ast {
	public class ExprBox : Expr {

		public ExprBox (MethodInfo methodInfo, Expr exprToBox)
			: base (methodInfo, ExprType.Box, exprToBox.ReturnType)
		{
			this.ExprToBox = exprToBox;
		}

		public Expr ExprToBox { get; private set; }

	}
}
