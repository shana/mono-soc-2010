﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using cccheck.Decompiler;

namespace cccheck.Ast {
	public class ExprReturn : Expr {

		public ExprReturn (MethodInfo methodInfo)
			: base (methodInfo)
		{
		}

		public override ExprType ExprType
		{
			get { return ExprType.Return; }
		}

		public override TypeReference ReturnType
		{
			get { return base.MethodInfo.TypeVoid; }
		}

	}
}
