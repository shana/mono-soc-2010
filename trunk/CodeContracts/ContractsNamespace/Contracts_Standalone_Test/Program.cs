using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Diagnostics.Contracts;
using System.Diagnostics;
using System.Reflection;
using System.Diagnostics.Contracts.Internal;
using ContractsTests.Helpers;

namespace ContractsTests {
    class Program {

        static void Main (string [] args)
        {

            var program = new Program ();
            program.asserts = new AssertListener ();
            Debug.Listeners.Clear ();
            Debug.Listeners.Add (program.asserts);

            program.SimulateTest ();
        }

        private AssertListener asserts;

        private void SimulateTest ()
        {

        }

    }
}
