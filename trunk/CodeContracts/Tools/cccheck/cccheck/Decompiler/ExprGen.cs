using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cccheck.Ast;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace cccheck.Decompiler {
	public class ExprGen {

		public ExprGen (MethodInfo methodInfo)
		{
			this.methodInfo = methodInfo;
		}

		private MethodInfo methodInfo;

		public ExprBlock Block (IEnumerable<Expr> exprs)
		{
			return new ExprBlock (this.methodInfo, exprs);
		}

		public ExprReturn Return ()
		{
			return new ExprReturn (this.methodInfo);
		}

		public ExprBox Box (Expr exprToBox)
		{
			return new ExprBox (this.methodInfo, exprToBox);
		}

		public ExprNop Nop ()
		{
			return new ExprNop (this.methodInfo);
		}

		public ExprLoadArg LoadArg (ParameterDefinition parameter)
		{
			return new ExprLoadArg (this.methodInfo, parameter);
		}

		public ExprLoadLocal LoadLocal (VariableDefinition variable)
		{
			return new ExprLoadLocal (this.methodInfo, variable);
		}

		public ExprStoreLocal StoreLocal (VariableDefinition variable, Expr value)
		{
			return new ExprStoreLocal (this.methodInfo, variable, value);
		}

		public ExprLoadConstant LoadConstant (object value)
		{
			return new ExprLoadConstant (this.methodInfo, value);
		}

		public ExprCall Call (MethodReference method, IEnumerable<Expr> parameters)
		{
			return new ExprCall (this.methodInfo, method, parameters);
		}


		public ExprCompareEqual CompareEqual (Expr left, Expr right)
		{
			return new ExprCompareEqual (this.methodInfo, left, right);
		}

		public ExprCompareLessThan CompareLessThan (Expr left, Expr right, Sn signage)
		{
			return new ExprCompareLessThan (this.methodInfo, left, right, signage);
		}

		public ExprCompareGreaterThan CompareGreaterThan (Expr left, Expr right, Sn signage)
		{
			return new ExprCompareGreaterThan (this.methodInfo, left, right, signage);
		}


		public ExprConv Conv (Expr exprToConvert, TypeCode convToType)
		{
			return new ExprConv (this.methodInfo, exprToConvert, convToType);
		}

		public ExprAdd Add (Expr left, Expr right, Sn signage, bool overflow)
		{
			return new ExprAdd (this.methodInfo, left, right, signage, overflow);
		}

		public ExprSub Sub (Expr left, Expr right, Sn signage, bool overflow)
		{
			return new ExprSub (this.methodInfo, left, right, signage, overflow);
		}

		public ExprLoadLength LoadLength (Expr array)
		{
			return new ExprLoadLength (this.methodInfo, array);
		}

	}
}
