#define CONTRACTS_FULL

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace CcCheckTest {
	class Program {

		static void Test (int a)
		{
			Contract.Requires (a > 0);
		}


		static void Main (string [] args)
		{
			Test (args.Length + 0);
		}
	}
}
