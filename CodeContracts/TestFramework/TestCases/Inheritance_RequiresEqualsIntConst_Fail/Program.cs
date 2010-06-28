#define CONTRACTS_FULL

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Utils;

namespace Inheritance_RequiresEqualsIntConst_OK {

	class A {

		public virtual void Test (int i)
		{
			Contract.Requires (i == 5);
		}

	}

	class B : A {

		public override void Test (int i)
		{
			// Do nothing
		}

	}

	class Program {
		static void Main (string [] args)
		{
			TestCaseRunner.Run (() => {
				B b = new B ();
				b.Test (6);
			});
		}
	}
}
