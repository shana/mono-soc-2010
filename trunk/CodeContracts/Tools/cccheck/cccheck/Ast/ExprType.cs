using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cccheck.Ast {
	public enum ExprType {

		Unknown,

		Block,

		Nop,
		LoadArg,
		LoadLocal,
		StoreLocal,
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

		LoadLength,

	}
}
