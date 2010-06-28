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
				case TypeCode.String:
					return this.il.Create (OpCodes.Ldstr, (string) v);
				default:
					throw new NotSupportedException ("Cannot handle constant: " + vTypeCode);
				}
			});

			return e;
		}

		private Expr VisitBinary (ExprBinaryOp e, Func<Instruction> fnCreateIl)
		{
			this.Visit (e.Left);
			this.Visit (e.Right);
			var inst = fnCreateIl ();
			this.Emit (inst);
			return e;
		}

		protected override Expr VisitCompareLessThan (ExprCompareLessThan e)
		{
			return this.VisitBinary (e, () => this.il.Create (e.IsSigned ? OpCodes.Clt : OpCodes.Clt_Un));
		}

		protected override Expr VisitCompareGreaterThan (ExprCompareGreaterThan e)
		{
			return this.VisitBinary (e, () => this.il.Create (e.IsSigned ? OpCodes.Cgt : OpCodes.Cgt_Un));
		}

		protected override Expr VisitCompareEqual (ExprCompareEqual e)
		{
			return this.VisitBinary (e, () => this.il.Create (OpCodes.Ceq));
		}

		protected override Expr VisitAdd (ExprAdd e)
		{
			return this.VisitBinary (e, () => {
				if (!e.Overflow) {
					return this.il.Create (OpCodes.Add);
				} else {
					return this.il.Create (e.IsSigned ? OpCodes.Add_Ovf : OpCodes.Add_Ovf_Un);
				}
			});
		}

		protected override Expr VisitSub (ExprSub e)
		{
			return this.VisitBinary (e, () => {
				if (!e.Overflow) {
					return this.il.Create (OpCodes.Sub);
				} else {
					return this.il.Create (e.IsSigned ? OpCodes.Sub_Ovf : OpCodes.Sub_Ovf_Un);
				}
			});
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

		protected override Expr VisitConv (ExprConv e)
		{
			this.Visit (e.ExprToConvert);
			Instruction instConv;
			switch (e.ConvToType) {
			case TypeCode.Int64:
				instConv = this.il.Create (OpCodes.Conv_I8);
				break;
			default:
				throw new NotSupportedException ("Cannot conv to: " + e.ConvToType);
			}
			this.Emit (instConv);
			return e;
		}

	}
}
