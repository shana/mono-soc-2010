using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Diagnostics.Contracts;
using System.Diagnostics;
using ContractsTests.Helpers;

namespace ContractsTests {

    [TestFixture]
    public class TestContractAssert : TestContractBase {

        /// <summary>
        /// Ensures that Assert(true) allows execution to continue.
        /// </summary>
        [Test, RunAgainstReference]
        public void TestAssertTrue ()
        {
            Contract.Assert (true);
            Assert.That (base.asserts.Asserts, Is.Empty);
        }

        /// <summary>
        /// Contract.Assert(false) will cause an assert to be triggered with the correct message.
        /// </summary>
        [Test]
        public void TestAssertNoEventHandler ()
        {
            Contract.Assert (false);
            Contract.Assert (false, "Message");

            Assert.That (base.asserts.Asserts.Count, Is.EqualTo (2));
            Assert.That (base.asserts.Asserts [0].Message, Is.EqualTo ("Description: Assertion failed."));
            Assert.That (base.asserts.Asserts [1].Message, Is.EqualTo ("Description: Assertion failed.  Message"));
        }

        /// <summary>
        /// Contract.Assert(true) will not call the ContractFailed event handler.
        /// Contract.Assert(false) will call the ContractFailed event handler.
        /// Because nothing is done in the event handler, an assert should be triggered.
        /// </summary>
        [Test]
        public void TestAssertEventHandlerNoAction ()
        {
            bool visitedEventHandler = false;
            Contract.ContractFailed += (sender, e) => {
                visitedEventHandler = true;
            };

            Contract.Assert (true);
            Assert.That (visitedEventHandler, Is.False);

            Contract.Assert (false);
            Assert.That (visitedEventHandler, Is.True);
            Assert.That (base.asserts.Asserts.Count, Is.EqualTo (1));
        }

        /// <summary>
        /// Event handler calls SetHandled(), so no assert should be triggered.
        /// </summary>
        [Test, RunAgainstReference]
        public void TestAssertEventHandlerSetHandled ()
        {
            Contract.ContractFailed += (sender, e) => {
                e.SetHandled ();
            };

            Contract.Assert (false);
            Assert.That (base.asserts.Asserts, Is.Empty);
        }

        /// <summary>
        /// Event handler calls SetUnwind(), so exception of type ContractException should be thrown.
        /// </summary>
        [Test, RunAgainstReference]
        public void TestAssertEventHandlerSetUnwind ()
        {
            Contract.ContractFailed += (sender, e) => {
                e.SetUnwind ();
            };

            Assert.That (() => {
                Contract.Assert (false);
            },
            Throws.InstanceOf (base.ContractExceptionType)
            .With.InnerException.Null);
        }

        /// <summary>
        /// Event handler calls SetHandled() and SetUnwind(), so exception of type ContractException should be thrown,
        /// as SetUnwind overrides SetHandled.
        /// </summary>
        [Test, RunAgainstReference]
        public void TestAssertEventHandlerSetUnwindHandled ()
        {
            Contract.ContractFailed += (sender, e) => {
                e.SetHandled ();
                e.SetUnwind ();
            };

            Assert.That (() => {
                Contract.Assert (false);
            },
            Throws.InstanceOf (base.ContractExceptionType)
            .With.InnerException.Null);
        }

        /// <summary>
        /// Event handler throws exception.
        /// ContractException is thrown by Contract.Assert(), with InnerException set to the thrown exception.
        /// </summary>
        [Test, RunAgainstReference]
        public void TestAssertEventHandlerThrows ()
        {
            Contract.ContractFailed += (sender, e) => {
                throw new ArgumentNullException ();
            };

            Assert.That (() => {
                Contract.Assert (false);
            },
            Throws.InstanceOf (base.ContractExceptionType)
            .With.InnerException.TypeOf<ArgumentNullException> ());
        }

        /// <summary>
        /// Multiple event handlers are registered. Check that both are called, and that the SetHandled()
        /// call in one of them is handled correctly.
        /// </summary>
        [Test, RunAgainstReference]
        public void TestAssertMultipleHandlers ()
        {
            bool visited1 = false, visited2 = false;

            Contract.ContractFailed += (sender, e) => {
                visited1 = true;
                Assert.That (e.Handled, Is.False);
                e.SetHandled ();
            };
            Contract.ContractFailed += (sender, e) => {
                visited2 = true;
                Assert.That (e.Handled, Is.True);
            };

            Contract.Assert (false);

            Assert.That (visited1, Is.True);
            Assert.That (visited2, Is.True);
        }

    }
}
