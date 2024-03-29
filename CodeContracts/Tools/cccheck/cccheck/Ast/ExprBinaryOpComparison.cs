﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using cccheck.Decompiler;

namespace cccheck.Ast {
	public abstract class ExprBinaryOpComparison : ExprBinaryOp {

		public ExprBinaryOpComparison (MethodInfo methodInfo, Expr left, Expr right, Sn signage)
			: base (methodInfo, left, right, signage)
		{
		}

		public override TypeReference ReturnType
		{
			get { return base.MethodInfo.TypeBoolean; }
		}

	}
}
