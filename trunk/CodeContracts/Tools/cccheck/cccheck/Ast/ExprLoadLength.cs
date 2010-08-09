using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cccheck.Decompiler;

namespace cccheck.Ast {
	public class ExprLoadLength : Expr {

		public ExprLoadLength (MethodInfo methodInfo, Expr array)
			: base (methodInfo)
		{
			this.Array = array;
		}

		public Expr Array { get; private set; }

		public override ExprType ExprType
		{
			get { return ExprType.LoadLength; }
		}

		public override Mono.Cecil.TypeReference ReturnType
		{
			get { return base.MethodInfo.TypeInt32; }
		}

	}
}
