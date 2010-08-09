using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cccheck.Ast;

namespace cccheck {
	class IntervalMethodCall {

		public IntervalMethodCall (ExprCall call, IEnumerable<IntervalDomain<int>> parameters)
		{
			this.Call = call;
			this.Parameters = parameters;
		}

		public ExprCall Call { get; private set; }
		public IEnumerable<IntervalDomain<int>> Parameters { get; private set; }

	}
}
