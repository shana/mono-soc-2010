using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using cccheck.Decompiler;

namespace cccheck.Ast {
	public class ExprLoadArg : Expr {

		public ExprLoadArg (MethodInfo methodInfo, ParameterDefinition parameter)
			: base (methodInfo)
		{
			this.Parameter = parameter;
		}

		public ParameterDefinition Parameter { get; private set; }

		public override ExprType ExprType
		{
			get { return ExprType.LoadArg; }
		}

		public override TypeReference ReturnType
		{
			get
			{
				return this.Parameter.ParameterType;
			}
		}

	}
}
