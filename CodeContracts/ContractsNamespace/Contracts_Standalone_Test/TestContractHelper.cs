using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Diagnostics.Contracts.Internal;
using System.Diagnostics.Contracts;
using ContractsTests.Helpers;
using NUnit.Framework.Constraints;

namespace ContractsTests {

    [TestFixture]
    public class TestContractHelper : TestContractBase {

        // Required when compiling/running under .NET3.5
        delegate void Action<T1, T2, T3, T4, T5> (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

        private void CheckAllMessages (ContractFailureKind kind, string messageStart, Action<string, Exception, string, ContractFailureKind, Func<string>> fnAssert)
        {

            foreach (Exception ex in new [] { null, new ArgumentNullException () }) {
                fnAssert (messageStart + ".", ex, null, kind, () => {
                    return ContractHelper.RaiseContractFailedEvent (kind, null, null, ex);
                });

                fnAssert (messageStart + ".  Message", ex, null, kind, () => {
                    return ContractHelper.RaiseContractFailedEvent (kind, "Message", null, ex);
                });

                fnAssert (messageStart + ": Condition", ex, "Condition", kind, () => {
                    return ContractHelper.RaiseContractFailedEvent (kind, null, "Condition", ex);
                });

                fnAssert (messageStart + ": Condition  Message", ex, "Condition", kind, () => {
                    return ContractHelper.RaiseContractFailedEvent (kind, "Message", "Condition", ex);
                });
            }

        }

        private void CheckAllKinds (Action<string, Exception, string, ContractFailureKind, Func<string>> fnAssert)
        {
            this.CheckAllMessages (ContractFailureKind.Assert, "Assertion failed", fnAssert);
            this.CheckAllMessages (ContractFailureKind.Assume, "Assumption failed", fnAssert);
            this.CheckAllMessages (ContractFailureKind.Invariant, "Invariant failed", fnAssert);
            this.CheckAllMessages (ContractFailureKind.Postcondition, "Postcondition failed", fnAssert);
            this.CheckAllMessages (ContractFailureKind.PostconditionOnException, "Postcondition failed after throwing an exception", fnAssert);
            this.CheckAllMessages (ContractFailureKind.Precondition, "Precondition failed", fnAssert);
        }

        private void CheckAllKinds (Action<string, Exception, Func<string>> fnAssert)
        {
            this.CheckAllKinds ((expected, ex, condition, kind, fnTest) => fnAssert (expected, ex, fnTest));
        }

        /// <summary>
        /// If no event handler is present, then the string returned describes the condition failure.
        /// </summary>
        [Test, RunAgainstReference]
        public void TestRaiseContractFailedEventNoHandler ()
        {
            this.CheckAllKinds ((expected, ex, fnTest) => {
                string msg = fnTest ();
                Assert.That (msg, Is.EqualTo (expected));
            });
        }

        /// <summary>
        /// When SetHandled() is called, null is returned.
        /// The event args are also checked.
        /// </summary>
        [Test, RunAgainstReference]
        public void TestRaiseContractFailedEventHandled ()
        {
            string expectedMsg = null;
            Exception originalException = null;
            string expectedCondition = null;
            ContractFailureKind expectedKind = ContractFailureKind.Assert;
            Contract.ContractFailed += (sender, e) => {
                Assert.That (e.Message, Is.EqualTo (expectedMsg));
                Assert.That (e.OriginalException, Is.SameAs (originalException));
                Assert.That (e.Condition, Is.EqualTo (expectedCondition));
                Assert.That (e.FailureKind, Is.EqualTo (expectedKind));
                e.SetHandled ();
            };

            this.CheckAllKinds ((expected, ex, condition, kind, fnTest) => {
                expectedMsg = expected;
                originalException = ex;
                expectedCondition = condition;
                expectedKind = kind;
                string msg = fnTest ();
                Assert.That (msg, Is.Null);
            });
        }

        /// <summary>
        /// When SetUnwind() is called, a ContractException is thrown. If an innerException is passed, then
        /// it is put in the InnerException of the ContractException. Otherwise, the InnerException is set to null.
        /// </summary>
        [Test, RunAgainstReference]
        public void TestRaiseContractFailedEventUnwind ()
        {
            Contract.ContractFailed += (sender, e) => {
                e.SetUnwind ();
            };

            this.CheckAllKinds ((expected, ex, fnTest) => {
                IResolveConstraint constraint = (ex == null) ?
                    (IResolveConstraint)Throws.InstanceOf (base.ContractExceptionType).With.InnerException.Null :
                    (IResolveConstraint)Throws.InstanceOf (base.ContractExceptionType).With.InnerException.TypeOf (ex.GetType ());
                Assert.That(()=>fnTest(), constraint);
            });
        }

        /// <summary>
        /// When the ContractFailed event throws an exception, then it becomes the inner exception of the
        /// ContractException. Except if an exception is passed in to the call, then that exception is put
        /// in the InnerException.
        /// </summary>
        [Test, RunAgainstReference]
        public void TestRaiseContractFailedEventThrows ()
        {
            Contract.ContractFailed += (sender, e) => {
                throw new InvalidOperationException ();
            };

            this.CheckAllKinds ((expected, ex, fnTest) => {
                Type expectedInnerExceptionType = ex == null ? typeof (InvalidOperationException) : ex.GetType ();
                Assert.That (() => fnTest (),
                    Throws.InstanceOf (base.ContractExceptionType)
                    .With.InnerException.TypeOf (expectedInnerExceptionType));
            });
        }

        /// <summary>
        /// Both event handlers should be called, constraint is not handled.
        /// </summary>
        [Test, RunAgainstReference]
        public void TestRaiseContractMultipleHandlers1 ()
        {
            bool visited1, visited2;
            Contract.ContractFailed += (sender, e) => {
                visited1 = true;
            };
            Contract.ContractFailed += (sender, e) => {
                visited2 = true;
            };

            this.CheckAllKinds ((expected, ex, fnTest) => {
                visited1 = visited2 = false;
                string msg = fnTest ();
                Assert.That (msg, Is.EqualTo (expected));
                Assert.That (visited1, Is.True);
                Assert.That (visited2, Is.True);
            });
        }

        /// <summary>
        /// Both event handlers should be called. SetUnwind() takes precedent over SetHandled().
        /// </summary>
        [Test, RunAgainstReference]
        public void TestRaiseContractMultipleHandlers2 ()
        {
            bool visited1, visited2;
            Contract.ContractFailed += (sender, e) => {
                visited1 = true;
                e.SetHandled ();
            };
            Contract.ContractFailed += (sender, e) => {
                visited2 = true;
                e.SetUnwind ();
            };

            this.CheckAllKinds ((expected, ex, fnTest) => {
                visited1 = visited2 = false;
                IResolveConstraint constraint = (ex == null) ?
                    (IResolveConstraint) Throws.InstanceOf (base.ContractExceptionType).With.InnerException.Null :
                    (IResolveConstraint) Throws.InstanceOf (base.ContractExceptionType).With.InnerException.TypeOf (ex.GetType ());
                Assert.That (() => fnTest (), constraint);
                Assert.That (visited1, Is.True);
                Assert.That (visited2, Is.True);
            });
        }

        /// <summary>
        /// Both event handlers should be called. The exceptions are treated as calls to SetUnwind(), with
        /// the exception being put in the InnerException.
        /// </summary>
        [Test, RunAgainstReference]
        public void TestRaiseContractMultipleHandlers3 ()
        {
            bool visited1, visited2;
            Contract.ContractFailed += (sender, e) => {
                visited1 = true;
                throw new InvalidOperationException ();
            };
            Contract.ContractFailed += (sender, e) => {
                visited2 = true;
                throw new InvalidOperationException ();
            };

            this.CheckAllKinds ((expected, ex, fnTest) => {
                visited1 = visited2 = false;
                Type expectedInnerExceptionType = ex == null ? typeof (InvalidOperationException) : ex.GetType ();
                Assert.That (() => fnTest (),
                    Throws.InstanceOf (base.ContractExceptionType)
                    .With.InnerException.TypeOf (expectedInnerExceptionType));
                Assert.That (visited1, Is.True);
                Assert.That (visited2, Is.True);
            });
        }

        /// <summary>
        /// Contract.TriggerFailure() triggers the assert. Check that the assert is triggered, with the correct text.
        /// </summary>
        [Test]
        public void TestTriggerFailure ()
        {
            ContractHelper.TriggerFailure (ContractFailureKind.Assert, "Display", null, "Condition", null);

            Assert.That (base.asserts.Asserts.Count, Is.EqualTo (1));
            var textLines = base.asserts.Asserts [0].Message.Split (new [] { Environment.NewLine }, StringSplitOptions.None);
            Assert.That (textLines [0], Is.EqualTo ("Expression: Condition"));
            Assert.That (textLines [1], Is.EqualTo ("Description: Display"));
        }

    }

}
