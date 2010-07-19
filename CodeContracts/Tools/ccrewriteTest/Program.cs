#define CONTRACTS_FULL

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace ccrewriteTest {
	class Program {
		static void Main (string [] args)
		{
			using (var r = new RewriteAndLoad ()) {
				r.Load ();
				r.Call (() => Test1 (0));
			}
		}

		static void Test1 (int i)
		{

			Contract.Requires (i == 4);
		}

	}
}
