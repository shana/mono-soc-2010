#define CONTRACTS_FULL

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Utils;

namespace Trivial_RequiresNotEqualsObjectConst_Fail {
    class Program {

        static void Test (object o)
        {
            Contract.Requires (o != null);
        }

        static void Main (string [] args)
        {
            TestCaseRunner.Run (true, () => {
                object o = null;
                Test (o);
            });
        }

    }
}
