#define CONTRACTS_FULL

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;
using System.Diagnostics.Contracts;

namespace Trivial_RequiresEqualsDoubleConst_OK {
    class Program {

        static void Test (double d)
        {
            Contract.Requires (d == 0.0);
        }

        static void Main (string [] args)
        {
            TestCaseRunner.Run (() => {
                double d = 0.0;
                Test (d);
            });
        }

    }
}
