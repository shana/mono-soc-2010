using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils {
    public static class TestCaseRunner {

        public static void Run (Action testCase)
        {
            try {
                testCase ();
            } catch (Exception ex) {
                Console.WriteLine (ex);
            }
        }

    }
}
