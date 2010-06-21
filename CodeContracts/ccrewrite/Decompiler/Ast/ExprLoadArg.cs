using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Decompiler.Ast {
	public class ExprLoadArg : Expr {

		public ExprLoadArg (MethodInfo methodInfo, int index)
			: base (methodInfo, ExprType.LoadArg)
		{
			this.Index = index;
		}

		public int Index { get; private set; }

		public override TypeReference ReturnType
		{
			get
			{
				return base.MethodInfo.Method.Parameters [this.Index].ParameterType;
			}
		}

	}
}
