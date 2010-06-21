using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Decompiler.Ast;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Decompiler {

	public class Decompile {

		public Decompile (MethodDefinition method)
		{
			this.method = method;
			this.exprs = new Stack<Expr> ();
			this.Instructions = new Dictionary<Expr, Instruction> ();
			this.methodInfo = new MethodInfo (method);
		}

		private MethodInfo methodInfo;
		private MethodDefinition method;
		private Stack<Expr> exprs;

		public Dictionary<Expr, Instruction> Instructions { get; private set; }

		public Expr Go ()
		{
			var insts = method.Body.Instructions;
			try {
				foreach (var inst in insts) {
					this.ProcessInst (inst);
				}
			} catch (NotSupportedException) {
				// Ignore for now, to allow basic rewriting to work.
			}

			Expr decompiled = new ExprBlock (this.methodInfo, this.exprs.Reverse ().ToArray ());
			return decompiled;
		}

		private void ProcessInst (Instruction inst)
		{
			Expr expr;
			switch (inst.OpCode.Code) {
			case Code.Nop:
				expr = this.ProcessNop ();
				break;
			case Code.Ldarg_0:
				expr = this.ProcessLoadArg (0);
				break;
			case Code.Ldarg_1:
				expr = this.ProcessLoadArg (1);
				break;
			case Code.Ldarg_2:
				expr = this.ProcessLoadArg (2);
				break;
			case Code.Ldarg_3:
				expr = this.ProcessLoadArg (3);
				break;
			case Code.Ldarg_S:
				expr = this.ProcessLoadArg (((ParameterDefinition) inst.Operand).Index);
				break;
			case Code.Ldc_I4_0:
				expr = this.ProcessLoadConstant (0);
				break;
			case Code.Ldc_R4:
			case Code.Ldc_R8:
				expr = this.ProcessLoadConstant (inst.Operand);
				break;
			case Code.Clt:
				expr = this.ProcessCompareLessThan (false);
				break;
			case Code.Clt_Un:
				expr = this.ProcessCompareLessThan (true);
				break;
			case Code.Cgt:
				expr = this.ProcessCompareGreaterThan (false);
				break;
			case Code.Cgt_Un:
				expr = this.ProcessCompareGreaterThan (true);
				break;
			case Code.Ceq:
				expr = this.ProcessCompareEqual ();
				break;
			case Code.Call:
				expr = this.ProcessCall ((MethodReference) inst.Operand);
				break;
			case Code.Ret:
				expr = this.ProcessReturn ();
				break;
			case Code.Conv_I8:
				expr = this.ProcessConvI8 ();
				break;
			default:
				throw new NotSupportedException ("Cannot handle opcode: " + inst.OpCode);
			}
			this.Instructions.Add (expr, inst);
			this.exprs.Push (expr);
		}

		private Expr ProcessNop ()
		{
			return new ExprNop (this.methodInfo);
		}

		private Expr ProcessLoadArg (int index)
		{
			return new ExprLoadArg (this.methodInfo, index);
		}

		private Expr ProcessLoadConstant (object value)
		{
			return new ExprLoadConstant (this.methodInfo, value);
		}

		private Expr ProcessCompareLessThan (bool unsigned)
		{
			Expr right = this.exprs.Pop ();
			Expr left = this.exprs.Pop ();
			return new ExprCompareLessThan (this.methodInfo, left, right, unsigned);
		}

		private Expr ProcessCompareGreaterThan (bool unsigned)
		{
			Expr right = this.exprs.Pop ();
			Expr left = this.exprs.Pop ();
			return new ExprCompareGreaterThan (this.methodInfo, left, right, unsigned);
		}

		private Expr ProcessCompareEqual ()
		{
			Expr right = this.exprs.Pop ();
			Expr left = this.exprs.Pop ();
			return new ExprCompareEqual (this.methodInfo, left, right);
		}

		private Expr ProcessCall (MethodReference method)
		{
			int paramCount = method.Parameters.Count;
			Expr [] parameterExprs = new Expr [paramCount];
			for (int i = 0; i < paramCount; i++) {
				Expr parameter = this.exprs.Pop ();
				parameterExprs [paramCount - i - 1] = parameter;
			}
			return new ExprCall (this.methodInfo, method, parameterExprs);
		}

		private Expr ProcessReturn ()
		{
			return new ExprReturn (this.methodInfo);
		}

		private Expr ProcessConvI8 ()
		{
			Expr exprToConvert = this.exprs.Pop ();
			return new ExprConvI8 (this.methodInfo, exprToConvert);
		}

	}
}
