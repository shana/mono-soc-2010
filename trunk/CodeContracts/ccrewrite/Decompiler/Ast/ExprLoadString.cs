using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Decompiler.Ast {
	public class ExprLoadString:Expr {

		public ExprLoadString (MethodInfo methodInfo, string value)
			: base (methodInfo, ExprType.LoadString, methodInfo.TypeString)
		{
			this.Value = value;
		}

		public string Value { get; private set; }

	}
}
