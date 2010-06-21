using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Decompiler.Ast {
	public enum ExprType {

		Block,

		Nop,
		LoadArg,
		LoadConstant,
		LoadString,
		CompareLessThan,
		CompareGreaterThan,
		CompareEqual,
		Call,
		Return,
		Box,
		ConvI8,

	}
}
