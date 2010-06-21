using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Decompiler.Ast {
	public class ExprReturn : Expr {

		public ExprReturn (MethodInfo methodInfo)
			: base (methodInfo, ExprType.Return, methodInfo.TypeVoid)
		{
		}

	}
}
