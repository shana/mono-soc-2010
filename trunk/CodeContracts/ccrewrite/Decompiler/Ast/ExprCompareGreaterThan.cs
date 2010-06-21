using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Decompiler.Ast {
	public class ExprCompareGreaterThan : Expr {

		public ExprCompareGreaterThan (MethodInfo methodInfo, Expr left, Expr right, bool unsigned)
			: base (methodInfo, ExprType.CompareGreaterThan, methodInfo.TypeBoolean)
		{
			this.Left = left;
			this.Right = right;
			this.Unsigned = unsigned;
		}

		public Expr Left { get; private set; }
		public Expr Right { get; private set; }
		public bool Unsigned { get; private set; }

	}
}
