using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Diagnostics.Contracts;
using ContractsTests.Helpers;

namespace ContractsTests {

    [TestFixture]
    class TestContractAssume : TestContractBase {

        /// <summary>
        /// At runtime Contract.Assume() acts just like a Contract.Assert(), except the exact message in the assert
        /// or exception is slightly different.
        /// </summary>
        [Test]
        public void TestAssumeMessage ()
        {
            Contract.Assume (false);
            Contract.Assume (false, "Message");

            Assert.That (this.asserts.Asserts.Count, Is.EqualTo (2));
            Assert.That (this.asserts.Asserts [0].Message, Is.EqualTo ("Description: Assumption failed."));
            Assert.That (this.asserts.Asserts [1].Message, Is.EqualTo ("Description: Assumption failed.  Message"));
        }

        // Identical to Contract.Assert, so no more testing required.

    }

}
