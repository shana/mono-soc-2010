using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Decompiler.Ast {
	public enum ExprType {

		Unknown,

		Block,

		Nop,
		LoadArg,
		LoadConstant,
		CompareLessThan,
		CompareGreaterThan,
		CompareEqual,
		Call,
		Return,
		Box,
		Conv,
		Add,
		Sub,

	}
}
