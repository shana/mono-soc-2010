using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Diagnostics.Contracts;
using ContractsTests.Helpers;

namespace ContractsTests {

    [TestFixture]
    public class TestContractMarkerMethods : TestContractBase {

        /// <summary>
        /// Contract.EndContractBlock() has no effects.
        /// </summary>
        [Test, RunAgainstReference]
        public void TestEndContractBlock ()
        {
            Contract.EndContractBlock ();
            Assert.That (base.asserts.Asserts, Is.Empty);
        }

        /// <summary>
        /// Contract.OldValue() has no effect, and always returns the default value for the type.
        /// </summary>
        [Test, RunAgainstReference]
        public void TestOldValue ()
        {
            int i = Contract.OldValue<int> (8);
            object o = Contract.OldValue<object> (new object ());

            Assert.That (base.asserts.Asserts, Is.Empty);
            Assert.That (i, Is.EqualTo (0));
            Assert.That (o, Is.Null);
        }

        /// <summary>
        /// Contract.Result() has no effect, and always returns the default value for the type.
        /// </summary>
        [Test, RunAgainstReference]
        public void TestResult ()
        {
            int i = Contract.Result<int> ();
            object o = Contract.Result<object> ();

            Assert.That (base.asserts.Asserts, Is.Empty);
            Assert.That (i, Is.EqualTo (0));
            Assert.That (o, Is.Null);
        }

        /// <summary>
        /// Contract.ValueAtReturn() has no effect, and always returns the default value for the type.
        /// </summary>
        [Test, RunAgainstReference]
        public void TestValueAtReturn ()
        {
            int iOut, i;
            object oOut, o;

            i = Contract.ValueAtReturn (out iOut);
            o = Contract.ValueAtReturn (out oOut);

            Assert.That (base.asserts.Asserts, Is.Empty);
            Assert.That (i, Is.EqualTo (0));
            Assert.That (o, Is.Null);
            Assert.That (iOut, Is.EqualTo (0));
            Assert.That (oOut, Is.Null);
        }

    }

}
