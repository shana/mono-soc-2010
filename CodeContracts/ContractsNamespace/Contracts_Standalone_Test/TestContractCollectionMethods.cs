using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ContractsTests.Helpers;
using System.Diagnostics.Contracts;

namespace ContractsTests {

    [TestFixture]
    public class TestContractCollectionMethods {

        /// <summary>
        /// Contract.Exists() determines that at least one element in the collection satisfies the predicate.
        /// </summary>
        [Test, RunAgainstReference]
        public void TestExistsInt ()
        {
            Assert.That (() => {
                Contract.Exists (0, 10, null);
            }, Throws.InstanceOf<ArgumentNullException> ());
            Assert.That (() => {
                Contract.Exists (10, 0, i => false);
            }, Throws.ArgumentException);

            Assert.That (Contract.Exists (0, 10, i => i <= 0), Is.True);
            Assert.That (Contract.Exists (0, 10, i => i >= 9), Is.True);
            Assert.That (Contract.Exists (0, 10, i => i < 0), Is.False);
            Assert.That (Contract.Exists (0, 10, i => i > 9), Is.False);
        }

        /// <summary>
        /// Contract.Exists() determines that at least one element in the collection satisfies the predicate.
        /// </summary>
        [Test, RunAgainstReference]
        public void TestExistsEnumeration ()
        {
            Assert.That (() => {
                Contract.Exists (Enumerable.Range (0, 10), null);
            }, Throws.InstanceOf<ArgumentNullException> ());
            Assert.That (() => {
                Contract.Exists<int> (null, x => false);
            }, Throws.InstanceOf<ArgumentNullException> ());

            var en = Enumerable.Range (0, 10);
            Assert.That (Contract.Exists (en, i => i <= 0), Is.True);
            Assert.That (Contract.Exists (en, i => i >= 9), Is.True);
            Assert.That (Contract.Exists (en, i => i < 0), Is.False);
            Assert.That (Contract.Exists (en, i => i > 9), Is.False);
        }

        /// <summary>
        /// Contract.ForAll() determines if all elements in the collection satisfy the predicate.
        /// </summary>
        [Test, RunAgainstReference]
        public void TestForAllInt ()
        {
            Assert.That (() => {
                Contract.ForAll (0, 10, null);
            }, Throws.InstanceOf<ArgumentNullException> ());
            Assert.That (() => {
                Contract.ForAll (10, 0, i => false);
            }, Throws.ArgumentException);

            Assert.That (Contract.ForAll (0, 10, i => i <= 9), Is.True);
            Assert.That (Contract.ForAll (0, 10, i => i >= 0), Is.True);
            Assert.That (Contract.ForAll (0, 10, i => i < 9), Is.False);
            Assert.That (Contract.ForAll (0, 10, i => i > 0), Is.False);
        }

        /// <summary>
        /// Contract.ForAll() determines if all elements in the collection satisfy the predicate.
        /// </summary>
        [Test, RunAgainstReference]
        public void TestForAllEnumeration ()
        {
            Assert.That (() => {
                Contract.ForAll (Enumerable.Range (0, 10), null);
            }, Throws.InstanceOf<ArgumentNullException> ());
            Assert.That (() => {
                Contract.ForAll<int> (null, x => false);
            }, Throws.InstanceOf<ArgumentNullException> ());

            var en = Enumerable.Range (0, 10);
            Assert.That (Contract.ForAll (en, i => i <= 9), Is.True);
            Assert.That (Contract.ForAll (en, i => i >= 0), Is.True);
            Assert.That (Contract.ForAll (en, i => i < 9), Is.False);
            Assert.That (Contract.ForAll (en, i => i > 0), Is.False);
        }

    }

}
