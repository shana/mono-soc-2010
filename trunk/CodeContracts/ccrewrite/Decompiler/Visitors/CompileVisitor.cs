using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil.Cil;
using Decompiler.Ast;

namespace Decompiler.Visitors {
	public class CompileVisitor : ExprVisitor {

		public CompileVisitor (ILProcessor il)
		{
			this.il = il;
			this.fnEmit = inst => this.il.Append (inst);
		}

		public CompileVisitor (ILProcessor il, Action<Instruction> fnEmit)
		{
			this.il = il;
			this.fnEmit = fnEmit;
		}

		private ILProcessor il;
		private Action<Instruction> fnEmit;

		private void Emit (Instruction inst)
		{
			this.fnEmit (inst);
		}

		private void Emit (Func<Instruction> fnCreateInstruction)
		{
			Instruction inst = fnCreateInstruction();
			this.Emit (inst);
		}

		private void Emit (Func<IEnumerable<Instruction>> fnCreateInstruction)
		{
			throw new NotImplementedException ();
		}

		protected override Expr VisitNop (ExprNop e)
		{
			var instNop = this.il.Create(OpCodes.Nop);
			this.Emit (instNop);
			return e;
		}

		protected override Expr VisitLoadArg (ExprLoadArg e)
		{
			this.Emit (() => {
				int index = e.Index;
				switch (e.Index) {
				case 0:
					return this.il.Create (OpCodes.Ldarg_0);
				case 1:
					return this.il.Create (OpCodes.Ldarg_1);
				case 2:
					return this.il.Create (OpCodes.Ldarg_2);
				case 3:
					return this.il.Create (OpCodes.Ldarg_3);
				default:
					if (e.Index <= 255) {
						return this.il.Create (OpCodes.Ldarg_S, (byte) e.Index);
					} else {
						return this.il.Create (OpCodes.Ldarg, e.Index);
					}
				}
			});
			
			return e;
		}

		protected override Expr VisitLoadConstant (ExprLoadConstant e)
		{
			this.Emit (() => {
				object v = e.Value;
				if (v == null) {
					return this.il.Create (OpCodes.Ldnull);
				}
				Type vType = v.GetType ();
				TypeCode vTypeCode = Type.GetTypeCode(vType);
				switch (vTypeCode) {
				case TypeCode.Int32:
					int value = (int) v;
					switch (value) {
					case -1:
						return this.il.Create (OpCodes.Ldc_I4_M1);
					case 0:
						return this.il.Create (OpCodes.Ldc_I4_0);
					case 1:
						return this.il.Create (OpCodes.Ldc_I4_1);
					case 2:
						return this.il.Create (OpCodes.Ldc_I4_2);
					case 3:
						return this.il.Create (OpCodes.Ldc_I4_3);
					case 4:
						return this.il.Create (OpCodes.Ldc_I4_4);
					case 5:
						return this.il.Create (OpCodes.Ldc_I4_5);
					case 6:
						return this.il.Create (OpCodes.Ldc_I4_6);
					case 7:
						return this.il.Create (OpCodes.Ldc_I4_7);
					case 8:
						return this.il.Create (OpCodes.Ldc_I4_8);
					default:
						if (value >= -128 && value <= 127) {
							return this.il.Create (OpCodes.Ldc_I4_S, (sbyte) value);
						} else {
							return this.il.Create (OpCodes.Ldc_I4, value);
						}
					}
				case TypeCode.Single:
					return this.il.Create (OpCodes.Ldc_R4, (float) v);
				case TypeCode.Double:
					return this.il.Create (OpCodes.Ldc_R8, (double) v);
				default:
					throw new NotSupportedException ("Cannot handle constant: " + vTypeCode);
				}
			});

			return e;
		}

		protected override Expr VisitCompareLessThan (ExprCompareLessThan e)
		{
			this.Visit (e.Left);
			this.Visit (e.Right);
			var instClt = this.il.Create (e.Unsigned ? OpCodes.Clt_Un : OpCodes.Clt);
			this.Emit (instClt);
			return e;
		}

		protected override Expr VisitCompareGreaterThan (ExprCompareGreaterThan e)
		{
			this.Visit (e.Left);
			this.Visit (e.Right);
			var instClt = this.il.Create (e.Unsigned ? OpCodes.Cgt_Un : OpCodes.Cgt);
			this.Emit (instClt);
			return e;
		}

		protected override Expr VisitCompareEqual (ExprCompareEqual e)
		{
			this.Visit (e.Left);
			this.Visit (e.Right);
			var instCeq = this.il.Create (OpCodes.Ceq);
			this.Emit (instCeq);
			return e;
		}

		protected override Expr VisitCall (ExprCall e)
		{
			foreach (var param in e.Parameters) {
				this.Visit (param);
			}
			var instCall = this.il.Create (OpCodes.Call, e.Method);
			this.Emit (instCall);
			return e;
		}

		protected override Expr VisitLoadString (ExprLoadString e)
		{
			var instLoadString = this.il.Create (OpCodes.Ldstr, e.Value);
			this.Emit (instLoadString);
			return e;
		}

		protected override Expr VisitReturn (ExprReturn e)
		{
			var instReturn = this.il.Create (OpCodes.Ret);
			this.Emit (instReturn);
			return e;
		}

		protected override Expr VisitBox (ExprBox e)
		{
			this.Visit (e.ExprToBox);
			var instBox = this.il.Create (OpCodes.Box, e.ReturnType);
			this.Emit (instBox);
			return e;
		}

		protected override Expr VisitConvI8 (ExprConvI8 e)
		{
			this.Visit (e.ExprToConvert);
			var instConvI8 = this.il.Create (OpCodes.Conv_I8);
			this.Emit (instConvI8);
			return e;
		}

	}
}
