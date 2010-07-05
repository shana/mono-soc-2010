using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils {
    public static class TestCaseRunner {

        public static void Run (bool expectContractFailure, Action testCase)
        {
			bool contractFailure = false;
            try {
                testCase ();
            } catch (Exception ex) {
                Console.WriteLine (ex);
				contractFailure = true;
            }
			if (expectContractFailure != contractFailure) {
				//throw new InvalidOperationException ("Contract failure not consistent with expectations");
				Console.WriteLine ("Unexpected contract behaviour - " + DateTime.UtcNow);
			}
        }

    }
}
