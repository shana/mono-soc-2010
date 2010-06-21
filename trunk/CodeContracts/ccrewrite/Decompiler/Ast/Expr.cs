using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Decompiler.Ast {
	public abstract class Expr {

		protected Expr (MethodInfo methodInfo, ExprType exprType, TypeReference returnType)
		{
			this.MethodInfo = methodInfo;
			this.ExprType = exprType;
			this.returnType = returnType;
		}

		protected Expr (MethodInfo methodInfo, ExprType exprType) : this (methodInfo, exprType, null) { }

		private TypeReference returnType;

		public ExprType ExprType { get; private set; }
		public MethodInfo MethodInfo { get; private set; }

		public virtual TypeReference ReturnType
		{
			get {
				if (this.returnType == null) {
					throw new InvalidOperationException ("ReturnType not specified");
				}
				return this.returnType;
			}
		}

	}
}
