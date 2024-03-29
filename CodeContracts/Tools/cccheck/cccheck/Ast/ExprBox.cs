﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cccheck.Decompiler;

namespace cccheck.Ast {
	public class ExprBox : Expr {

		public ExprBox (MethodInfo methodInfo, Expr exprToBox)
			: base (methodInfo)
		{
			this.ExprToBox = exprToBox;
		}

		public override ExprType ExprType
		{
			get { return ExprType.Box; }
		}

		public override Mono.Cecil.TypeReference ReturnType
		{
			get { return this.ExprToBox.ReturnType; }
		}

		public Expr ExprToBox { get; private set; }

	}
}
