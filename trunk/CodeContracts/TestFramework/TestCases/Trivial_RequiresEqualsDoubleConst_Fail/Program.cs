using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Utils;

namespace Trivial_RequiresEqualsDoubleConst_Fail {
    class Program {

        static void Test (double d)
        {
            Contract.Requires (d == 0.0);
        }

        static void Main (string [] args)
        {
            TestCaseRunner.Run (() => {
                double d = 1.0;
                Test (d);
            });
        }

    }
}
