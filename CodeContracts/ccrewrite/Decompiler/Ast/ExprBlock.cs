using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Decompiler.Ast {
	public class ExprBlock : Expr {

		public ExprBlock (MethodInfo methodInfo, IEnumerable<Expr> exprs)
			: base (methodInfo, ExprType.Block, methodInfo.TypeVoid)
		{
			this.Exprs = exprs;
		}

		public IEnumerable<Expr> Exprs { get; private set; }

	}
}
