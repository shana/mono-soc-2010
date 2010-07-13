﻿#define CONTRACTS_FULL

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Utils;

namespace Trivial_RequiresNotEqualsObjectConst_OK {
    class Program {

        static void Test (object o)
        {
            Contract.Requires (o != null);
        }

        static void Main (string [] args)
        {
            TestCaseRunner.Run (false, () => {
                object o = new object();
                Test (o);
            });
        }

    }
}