#define CONTRACTS_FULL

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;
using System.Diagnostics.Contracts;

namespace Trivial_RequiresEqualsFloatConst_OK {
    class Program {

        static void Test (float f)
        {
            Contract.Requires (f == 0.0f);
        }

        static void Main (string [] args)
        {
            TestCaseRunner.Run (false, () => {
                float f = 0.0f;
                Test (f);
            });
        }

    }
}
