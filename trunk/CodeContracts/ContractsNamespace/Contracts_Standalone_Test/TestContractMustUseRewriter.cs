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

        private void CheckMustUseRewriter (Action fnTest, string expectedMsg)
        {
            fnTest ();

            bool handlerVisited = false;
            Contract.ContractFailed += (sender, e) => {
                handlerVisited = true;
            };

            fnTest ();

            Assert.That (handlerVisited, Is.False);
            Assert.That (this.asserts.Asserts.Count, Is.EqualTo (8));
            Assert.That (this.asserts.Asserts.Select (x => x.Message), Is.All.EqualTo (expectedMsg));
        }

        /// <summary>
        /// Contract.Requires() ALWAYS triggers an assert, regardless of any other factors.
        /// </summary>
        [Test]
        public void TestRequires ()
        {
            CheckMustUseRewriter (() => {
                Contract.Requires (true);
                Contract.Requires (false);
                Contract.Requires (true, "Message");
                Contract.Requires (false, "Message");
            },
                "Description: Must use the rewriter when using Contract.Requires");
        }

        /// <summary>
        /// Contract.Requires() ALWAYS triggers an assert, regardless of any other factors.
        /// </summary>
        [Test]
        public void TestRequiresTException ()
        {
            CheckMustUseRewriter (() => {
                Contract.Requires<Exception> (true);
                Contract.Requires<Exception> (false);
                Contract.Requires<Exception> (true, "Message");
                Contract.Requires<Exception> (false, "Message");
            },
                "Description: Must use the rewriter when using Contract.Requires<TException>");
        }

        /// <summary>
        /// Contract.Ensures() ALWAYS triggers an assert, regardless of any other factors.
        /// </summary>
        [Test]
        public void TestEnsures ()
        {
            CheckMustUseRewriter (() => {
                Contract.Ensures (true);
                Contract.Ensures (false);
                Contract.Ensures (true, "Message");
                Contract.Ensures (false, "Message");
            },
                "Description: Must use the rewriter when using Contract.Ensures");
        }

        /// <summary>
        /// Contract.Ensures() ALWAYS triggers an assert, regardless of any other factors.
        /// </summary>
        [Test]
        public void TestEnsuresOnThrow ()
        {
            CheckMustUseRewriter (() => {
                Contract.EnsuresOnThrow<Exception> (true);
                Contract.EnsuresOnThrow<Exception> (false);
                Contract.EnsuresOnThrow<Exception> (true, "Message");
                Contract.EnsuresOnThrow<Exception> (false, "Message");
            },
                "Description: Must use the rewriter when using Contract.EnsuresOnThrow");
        }

        /// <summary>
        /// Contract.Ensures() ALWAYS triggers an assert, regardless of any other factors.
        /// </summary>
        [Test]
        public void TestInvariant ()
        {
            CheckMustUseRewriter (() => {
                Contract.Invariant (true);
                Contract.Invariant (false);
                Contract.Invariant (true, "Message");
                Contract.Invariant (false, "Message");
            },
                "Description: Must use the rewriter when using Contract.Invariant");
        }

    }

}
