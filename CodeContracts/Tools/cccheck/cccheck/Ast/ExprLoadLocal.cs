using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cccheck.Decompiler;
using Mono.Cecil.Cil;

namespace cccheck.Ast {
	public class ExprLoadLocal : Expr {

		public ExprLoadLocal (MethodInfo methodInfo, VariableDefinition variable)
			: base (methodInfo)
		{
			this.Variable = variable;
		}

		public VariableDefinition Variable { get; private set; }

		public override ExprType ExprType
		{
			get { return ExprType.LoadLocal; }
		}

		public override Mono.Cecil.TypeReference ReturnType
		{
			get { return this.Variable.VariableType; }
		}

	}
}
