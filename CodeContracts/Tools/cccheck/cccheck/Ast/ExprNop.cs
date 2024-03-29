﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cccheck.Decompiler;

namespace cccheck.Ast {
	public class ExprNop :Expr {

		public ExprNop (MethodInfo methodInfo)
			: base (methodInfo)
		{
		}

		public override ExprType ExprType
		{
			get { return ExprType.Nop; }
		}

		public override Mono.Cecil.TypeReference ReturnType
		{
			get { return base.MethodInfo.TypeVoid; }
		}

	}
}
