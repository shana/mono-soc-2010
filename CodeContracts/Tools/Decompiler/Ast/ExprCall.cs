﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Decompiler.Ast {
	public class ExprCall : Expr {

		public ExprCall (MethodInfo methodInfo, MethodReference method, IEnumerable<Expr> parameters)
			: base (methodInfo)
		{
			this.Method = method;
			this.Parameters = parameters;
		}

		public MethodReference Method { get; private set; }
		public IEnumerable<Expr> Parameters { get; private set; }

		public override ExprType ExprType
		{
			get { return ExprType.Call; }
		}

		public override TypeReference ReturnType
		{
			get { return this.Method.ReturnType; }
		}

	}
}