using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Decompiler.Ast {
	public class ExprNop :Expr {

		public ExprNop (MethodInfo methodInfo)
			: base (methodInfo, ExprType.Nop, methodInfo.TypeVoid)
		{
		}

	}
}
