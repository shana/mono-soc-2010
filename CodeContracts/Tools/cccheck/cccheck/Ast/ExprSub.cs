﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cccheck.Decompiler;

namespace cccheck.Ast {
	public class ExprSub : ExprBinaryOpArithmetic {

		public ExprSub (MethodInfo methodInfo, Expr left, Expr right, Sn signage, bool overflow)
			: base (methodInfo, left, right, signage, overflow)
		{
		}

		public override ExprType ExprType
		{
			get { return ExprType.Sub; }
		}

	}
}
