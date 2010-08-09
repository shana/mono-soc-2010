using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cccheck.Decompiler;
using Mono.Cecil.Cil;
using Mono.Cecil;

namespace cccheck.Ast {
	public class ExprStoreLocal : Expr {

		public ExprStoreLocal (MethodInfo methodInfo, VariableDefinition variable, Expr value)
			: base (methodInfo)
		{
			this.Variable = variable;
			this.Value = value;
		}

		public VariableDefinition Variable { get; private set; }
		public Expr Value { get; private set; }

		public override ExprType ExprType
		{
			get { return ExprType.StoreLocal; }
		}

		public override TypeReference ReturnType
		{
			get { return this.Variable.VariableType; }
		}
	}
}
