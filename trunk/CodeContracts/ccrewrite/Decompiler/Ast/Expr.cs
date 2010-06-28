﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Decompiler.Ast {
	public abstract class Expr {

		protected Expr (MethodInfo methodInfo)
		{
			this.MethodInfo = methodInfo;
		}

		public abstract ExprType ExprType { get; }
		public abstract TypeReference ReturnType { get; }

		public MethodInfo MethodInfo { get; private set; }


	}
}
