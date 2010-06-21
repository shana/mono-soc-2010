using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Decompiler.Ast {
	public class ExprCompareEqual : Expr {

		public ExprCompareEqual (MethodInfo methodInfo, Expr left, Expr right)
			: base (methodInfo, ExprType.CompareEqual, methodInfo.TypeBoolean)
		{
			this.Left = left;
			this.Right = right;
		}

		public Expr Left { get; private set; }
		public Expr Right { get; private set; }

	}
}
