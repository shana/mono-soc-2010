using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Diagnostics.Contracts;
using ContractsTests.Helpers;

namespace ContractsTests {

    [TestFixture]
    public class TestContractRequires : TestContractBase {

        /// <summary>
        /// Contract.Requires() ALWAYS triggers an assert, regardless of any other factors.
        /// </summary>
        [Test]
        public void TestRequires ()
        {
            Contract.Requires (true);
            Contract.Requires (false);
            Contract.Requires (true, "Message");
            Contract.Requires (false, "Message");

            bool handlerVisited = false;
            Contract.ContractFailed += (sender, e) => {
                handlerVisited = true;
                e.SetHandled ();
                e.SetUnwind ();
            };

            Contract.Requires (true);
            Contract.Requires (false);
            Contract.Requires (true, "Message");
            Contract.Requires (false, "Message");

            Assert.That (handlerVisited, Is.False);
            Assert.That (this.asserts.Asserts.Count, Is.EqualTo (8));
            const string assertMsg = "Description: Must use the rewriter when using Contract.Requires";
            Assert.That (this.asserts.Asserts.Select (x => x.Message), Is.All.EqualTo (assertMsg));
        }

    }

}
