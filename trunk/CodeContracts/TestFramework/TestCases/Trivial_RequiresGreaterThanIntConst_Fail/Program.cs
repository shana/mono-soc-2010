#define CONTRACTS_FULL

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Utils;

namespace Trivial_RequiresGreaterThanIntConst_Fail {
    class Program {

        static void Test (int i)
        {
            Contract.Requires (i > 10);
        }

        static void Main (string [] args)
        {
            TestCaseRunner.Run (true, () => {
                int i = 10;
                Test (i);
            });
        }

    }
}
