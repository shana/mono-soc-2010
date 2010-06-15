Index: corlib_test.dll.sources
===================================================================
--- corlib_test.dll.sources	(revision 158944)
+++ corlib_test.dll.sources	(working copy)
@@ -60,6 +60,14 @@
 System.Diagnostics/StackFrameTest.cs
 System.Diagnostics/StackTraceTest.cs
 System.Diagnostics/TextWriterTraceListenerTest.cs
+System.Diagnostics.Contracts/ContractAssertTest.cs
+System.Diagnostics.Contracts/ContractAssumeTest.cs
+System.Diagnostics.Contracts/ContractCollectionMethodsTest.cs
+System.Diagnostics.Contracts/ContractHelperTest.cs
+System.Diagnostics.Contracts/ContractMarkerMethodsTest.cs
+System.Diagnostics.Contracts/ContractMustUseRewriterTest.cs
+System.Diagnostics.Contracts/Helpers/RunAgainstReferenceAttribute.cs
+System.Diagnostics.Contracts/Helpers/TestContractBase.cs
 System/DoubleFormatterTest.cs
 System/DoubleTest.cs
 System/EnumTest.cs
Index: Test/System.Diagnostics.Contracts/ContractAssumeTest.cs
===================================================================
--- Test/System.Diagnostics.Contracts/ContractAssumeTest.cs	(revision 0)
+++ Test/System.Diagnostics.Contracts/ContractAssumeTest.cs	(revision 0)
@@ -0,0 +1,45 @@
+#define CONTRACTS_FULL
+#define DEBUG
+
+#if NET_4_0
+using System;
+using System.Collections.Generic;
+using System.Linq;
+using System.Text;
+using NUnit.Framework;
+using System.Diagnostics.Contracts;
+using MonoTests.System.Diagnostics.Contracts.Helpers;
+
+namespace MonoTests.System.Diagnostics.Contracts {
+
+    [TestFixture]
+    public class ContractAssumeTest : TestContractBase {
+
+        /// <summary>
+        /// At runtime Contract.Assume() acts just like a Contract.Assert(), except the exact message in the assert
+        /// or exception is slightly different.
+        /// </summary>
+        [Test]
+        public void TestAssumeMessage ()
+        {
+            try {
+                Contract.Assume (false);
+                Assert.Fail ("TestAssumeMessage() exception not thrown #1");
+            } catch (Exception ex) {
+                Assert.IsInstanceOfType (typeof(NotImplementedException), ex, "TestAssumeMessage() wrong exception type #1");
+            }
+
+            try {
+                Contract.Assume (false, "Message");
+                Assert.Fail ("TestAssumeMessage() exception not thrown #1");
+            } catch (Exception ex) {
+                Assert.IsInstanceOfType (typeof(NotImplementedException), ex, "TestAssumeMessage() wrong exception type #1");
+            }
+        }
+
+        // Identical to Contract.Assert, so no more testing required.
+
+    }
+
+}
+#endif

Property changes on: Test/System.Diagnostics.Contracts/ContractAssumeTest.cs
___________________________________________________________________
Added: svn:eol-style
   + native

Index: Test/System.Diagnostics.Contracts/ContractCollectionMethodsTest.cs
===================================================================
--- Test/System.Diagnostics.Contracts/ContractCollectionMethodsTest.cs	(revision 0)
+++ Test/System.Diagnostics.Contracts/ContractCollectionMethodsTest.cs	(revision 0)
@@ -0,0 +1,127 @@
+#define CONTRACTS_FULL
+#define DEBUG
+
+#if NET_4_0
+using System;
+using System.Collections.Generic;
+using System.Linq;
+using System.Text;
+using NUnit.Framework;
+using MonoTests.System.Diagnostics.Contracts.Helpers;
+using System.Diagnostics.Contracts;
+
+namespace MonoTests.System.Diagnostics.Contracts {
+
+    [TestFixture]
+    public class ContractCollectionMethodsTest {
+
+        /// <summary>
+        /// Contract.Exists() determines that at least one element in the collection satisfies the predicate.
+        /// </summary>
+        [Test, RunAgainstReference]
+        public void TestExistsInt ()
+        {
+            try {
+                Contract.Exists (0, 10, null);
+                Assert.Fail ("TestExistsInt() no exception #1");
+            } catch (Exception ex) {
+                Assert.IsInstanceOfType (typeof (ArgumentNullException), ex, "TestExistsInt() wrong exception #1");
+            }
+
+            try {
+                Contract.Exists (10, 0, i => false);
+                Assert.Fail ("TestExistsInt() no exception #2");
+            } catch (Exception ex) {
+                Assert.IsInstanceOfType (typeof (ArgumentException), ex, "TestExistsInt() wrong exception #2");
+            }
+
+            Assert.IsTrue (Contract.Exists (0, 10, i => i <= 0), "TestExistsInt() #1");
+            Assert.IsTrue (Contract.Exists (0, 10, i => i >= 9), "TestExistsInt() #2");
+            Assert.IsFalse (Contract.Exists (0, 10, i => i < 0), "TestExistsInt() #3");
+            Assert.IsFalse (Contract.Exists (0, 10, i => i > 9), "TestExistsInt() #4");
+        }
+
+        /// <summary>
+        /// Contract.Exists() determines that at least one element in the collection satisfies the predicate.
+        /// </summary>
+        [Test, RunAgainstReference]
+        public void TestExistsEnumeration ()
+        {
+            try {
+                Contract.Exists (Enumerable.Range (0, 10), null);
+                Assert.Fail ("TestExistsEnumeration() no exception #1");
+            } catch (Exception ex) {
+                Assert.IsInstanceOfType (typeof (ArgumentNullException), ex, "TestExistsEnumeration() wrong exception #1");
+            }
+
+            try {
+                Contract.Exists<int> (null, x => false);
+                Assert.Fail ("TestExistsEnumeration() no exception #2");
+            } catch (Exception ex) {
+                Assert.IsInstanceOfType (typeof (ArgumentNullException), ex, "TestExistsEnumeration() wrong exception #2");
+            }
+
+            var en = Enumerable.Range (0, 10);
+            Assert.IsTrue (Contract.Exists (en, i => i <= 0), "TestExistsEnumeration() #1");
+            Assert.IsTrue (Contract.Exists (en, i => i >= 9), "TestExistsEnumeration() #2");
+            Assert.IsFalse (Contract.Exists (en, i => i < 0), "TestExistsEnumeration() #3");
+            Assert.IsFalse (Contract.Exists (en, i => i > 9), "TestExistsEnumeration() #4");
+        }
+
+        /// <summary>
+        /// Contract.ForAll() determines if all elements in the collection satisfy the predicate.
+        /// </summary>
+        [Test, RunAgainstReference]
+        public void TestForAllInt ()
+        {
+            try {
+                Contract.ForAll (0, 10, null);
+                Assert.Fail ("TestForAllInt() no exception #1");
+            } catch (Exception ex) {
+                Assert.IsInstanceOfType (typeof (ArgumentNullException), ex, "TestForAllInt() wrong exception #1");
+            }
+
+            try {
+                Contract.ForAll (10, 0, i => false);
+                Assert.Fail ("TestForAllInt() no exception #2");
+            } catch (Exception ex) {
+                Assert.IsInstanceOfType (typeof (ArgumentException), ex, "TestForAllInt() wrong exception #2");
+            }
+
+            Assert.IsTrue (Contract.ForAll (0, 10, i => i <= 9), "TestForAllInt() #1");
+            Assert.IsTrue (Contract.ForAll (0, 10, i => i >= 0), "TestForAllInt() #2");
+            Assert.IsFalse (Contract.ForAll (0, 10, i => i < 9), "TestForAllInt() #3");
+            Assert.IsFalse (Contract.ForAll (0, 10, i => i > 0), "TestForAllInt() #4");
+        }
+
+        /// <summary>
+        /// Contract.ForAll() determines if all elements in the collection satisfy the predicate.
+        /// </summary>
+        [Test, RunAgainstReference]
+        public void TestForAllEnumeration ()
+        {
+            try {
+                Contract.ForAll (Enumerable.Range (0, 10), null);
+                Assert.Fail ("TestForAllEnumeration() no exception #1");
+            } catch (Exception ex) {
+                Assert.IsInstanceOfType (typeof (ArgumentNullException), ex, "TestForAllEnumeration() wrong exception #1");
+            }
+
+            try {
+                Contract.ForAll<int> (null, x => false);
+                Assert.Fail ("TestForAllEnumeration() no exception #2");
+            } catch (Exception ex) {
+                Assert.IsInstanceOfType (typeof (ArgumentNullException), ex, "TestForAllEnumeration() wrong exception #2");
+            }
+
+            var en = Enumerable.Range (0, 10);
+            Assert.IsTrue (Contract.ForAll (en, i => i <= 9), "TestForAllEnumeration() #1");
+            Assert.IsTrue (Contract.ForAll (en, i => i >= 0), "TestForAllEnumeration() #2");
+            Assert.IsFalse (Contract.ForAll (en, i => i < 9), "TestForAllEnumeration() #3");
+            Assert.IsFalse (Contract.ForAll (en, i => i > 0), "TestForAllEnumeration() #4");
+        }
+
+    }
+
+}
+#endif

Property changes on: Test/System.Diagnostics.Contracts/ContractCollectionMethodsTest.cs
___________________________________________________________________
Added: svn:eol-style
   + native

Index: Test/System.Diagnostics.Contracts/ContractHelperTest.cs
===================================================================
--- Test/System.Diagnostics.Contracts/ContractHelperTest.cs	(revision 0)
+++ Test/System.Diagnostics.Contracts/ContractHelperTest.cs	(revision 0)
@@ -0,0 +1,259 @@
+#define CONTRACTS_FULL
+#define DEBUG
+
+#if NET_4_0
+using System;
+using System.Collections.Generic;
+using System.Linq;
+using System.Text;
+using NUnit.Framework;
+using System.Diagnostics.Contracts.Internal;
+using System.Diagnostics.Contracts;
+using MonoTests.System.Diagnostics.Contracts.Helpers;
+using NUnit.Framework.Constraints;
+
+namespace MonoTests.System.Diagnostics.Contracts {
+
+    [TestFixture]
+    public class ContractHelperTest : TestContractBase {
+
+        // Required when compiling/running under .NET3.5
+        delegate void Action<T1, T2, T3, T4, T5> (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
+
+        private void CheckAllMessages (ContractFailureKind kind, string messageStart, Action<string, Exception, string, ContractFailureKind, Func<string>> fnAssert)
+        {
+
+            foreach (Exception ex in new [] { null, new ArgumentNullException () }) {
+                fnAssert (messageStart + ".", ex, null, kind, () => {
+                    return ContractHelper.RaiseContractFailedEvent (kind, null, null, ex);
+                });
+
+                fnAssert (messageStart + ".  Message", ex, null, kind, () => {
+                    return ContractHelper.RaiseContractFailedEvent (kind, "Message", null, ex);
+                });
+
+                fnAssert (messageStart + ": Condition", ex, "Condition", kind, () => {
+                    return ContractHelper.RaiseContractFailedEvent (kind, null, "Condition", ex);
+                });
+
+                fnAssert (messageStart + ": Condition  Message", ex, "Condition", kind, () => {
+                    return ContractHelper.RaiseContractFailedEvent (kind, "Message", "Condition", ex);
+                });
+            }
+
+        }
+
+        private void CheckAllKinds (Action<string, Exception, string, ContractFailureKind, Func<string>> fnAssert)
+        {
+            this.CheckAllMessages (ContractFailureKind.Assert, "Assertion failed", fnAssert);
+            this.CheckAllMessages (ContractFailureKind.Assume, "Assumption failed", fnAssert);
+            this.CheckAllMessages (ContractFailureKind.Invariant, "Invariant failed", fnAssert);
+            this.CheckAllMessages (ContractFailureKind.Postcondition, "Postcondition failed", fnAssert);
+            this.CheckAllMessages (ContractFailureKind.PostconditionOnException, "Postcondition failed after throwing an exception", fnAssert);
+            this.CheckAllMessages (ContractFailureKind.Precondition, "Precondition failed", fnAssert);
+        }
+
+        private void CheckAllKinds (Action<string, Exception, Func<string>> fnAssert)
+        {
+            this.CheckAllKinds ((expected, ex, condition, kind, fnTest) => fnAssert (expected, ex, fnTest));
+        }
+
+        /// <summary>
+        /// If no event handler is present, then the string returned describes the condition failure.
+        /// </summary>
+        [Test, RunAgainstReference]
+        public void TestRaiseContractFailedEventNoHandler ()
+        {
+            this.CheckAllKinds ((expected, ex, fnTest) => {
+                string msg = fnTest ();
+                Assert.AreEqual (expected, msg, "TestRaiseContractFailedEventNoHandler() incorrect message");
+            });
+        }
+
+        /// <summary>
+        /// When SetHandled() is called, null is returned.
+        /// The event args are also checked.
+        /// </summary>
+        [Test, RunAgainstReference]
+        public void TestRaiseContractFailedEventHandled ()
+        {
+            string expectedMsg = null;
+            Exception originalException = null;
+            string expectedCondition = null;
+            ContractFailureKind expectedKind = ContractFailureKind.Assert;
+            Contract.ContractFailed += (sender, e) => {
+                Assert.AreEqual (expectedMsg, e.Message, "TestRaiseContractFailedEventHandled() event message wrong");
+                Assert.AreSame (originalException, e.OriginalException, "TestRaiseContractFailedEventHandled() event exception wrong");
+                Assert.AreEqual (expectedCondition, e.Condition, "TestRaiseContractFailedEventHandled() event condition wrong");
+                Assert.AreEqual (expectedKind, e.FailureKind, "TestRaiseContractFailedEventHandled() event failure kind wrong");
+                e.SetHandled ();
+            };
+
+            this.CheckAllKinds ((expected, ex, condition, kind, fnTest) => {
+                expectedMsg = expected;
+                originalException = ex;
+                expectedCondition = condition;
+                expectedKind = kind;
+                string msg = fnTest ();
+                Assert.IsNull (msg, "TestRaiseContractFailedEventHandled() expected null message");
+            });
+        }
+
+        /// <summary>
+        /// When SetUnwind() is called, a ContractException is thrown. If an innerException is passed, then
+        /// it is put in the InnerException of the ContractException. Otherwise, the InnerException is set to null.
+        /// </summary>
+        [Test, RunAgainstReference]
+        public void TestRaiseContractFailedEventUnwind ()
+        {
+            Contract.ContractFailed += (sender, e) => {
+                e.SetUnwind ();
+            };
+
+            this.CheckAllKinds ((expected, expectedEx, fnTest) => {
+                try {
+                    fnTest ();
+                    Assert.Fail ("TestRaiseContractFailedEventUnwind() exception not thrown");
+                } catch (Exception ex) {
+                    Assert.IsInstanceOfType (base.ContractExceptionType, ex, "TestRaiseContractFailedEventUnwind() wrong exception type");
+                    if (expectedEx == null) {
+                        Assert.IsNull (ex.InnerException, "TestRaiseContractFailedEventUnwind() inner exception should be null");
+                    } else {
+                        Assert.AreSame (expectedEx, ex.InnerException, "TestRaiseContractFailedEventUnwind() wrong inner exception type");
+                    }
+                }
+            });
+        }
+
+        /// <summary>
+        /// When the ContractFailed event throws an exception, then it becomes the inner exception of the
+        /// ContractException. Except if an exception is passed in to the call, then that exception is put
+        /// in the InnerException.
+        /// </summary>
+        [Test, RunAgainstReference]
+        public void TestRaiseContractFailedEventThrows ()
+        {
+            Contract.ContractFailed += (sender, e) => {
+                throw new InvalidOperationException ();
+            };
+
+            this.CheckAllKinds ((expected, expectedEx, fnTest) => {
+                try {
+                    fnTest ();
+                    Assert.Fail ("TestRaiseContractFailedEventThrows() exception not thrown");
+                } catch (Exception ex) {
+                    Assert.IsInstanceOfType (base.ContractExceptionType, ex, "TestRaiseContractFailedEventThrows() wrong exception type");
+                    Type expectedInnerExceptionType = expectedEx == null ? typeof (InvalidOperationException) : expectedEx.GetType ();
+                    Assert.IsInstanceOfType (expectedInnerExceptionType, ex.InnerException, "TestRaiseContractFailedEventThrows() wrong inner exception type");
+                }
+            });
+        }
+
+        /// <summary>
+        /// Both event handlers should be called, constraint is not handled.
+        /// </summary>
+        [Test, RunAgainstReference]
+        public void TestRaiseContractMultipleHandlers1 ()
+        {
+            bool visited1, visited2;
+            Contract.ContractFailed += (sender, e) => {
+                visited1 = true;
+            };
+            Contract.ContractFailed += (sender, e) => {
+                visited2 = true;
+            };
+
+            this.CheckAllKinds ((expected, ex, fnTest) => {
+                visited1 = visited2 = false;
+                string msg = fnTest ();
+                Assert.AreEqual (expected, msg, "TestRaiseContractMultipleHandlers1() msg not as expected");
+                Assert.IsTrue (visited1, "TestRaiseContractMultipleHandlers1() handler 1 not visited");
+                Assert.IsTrue (visited2, "TestRaiseContractMultipleHandlers1() handler 2 not visited");
+            });
+        }
+
+        /// <summary>
+        /// Both event handlers should be called. SetUnwind() takes precedent over SetHandled().
+        /// </summary>
+        [Test, RunAgainstReference]
+        public void TestRaiseContractMultipleHandlers2 ()
+        {
+            bool visited1, visited2;
+            Contract.ContractFailed += (sender, e) => {
+                visited1 = true;
+                e.SetHandled ();
+            };
+            Contract.ContractFailed += (sender, e) => {
+                visited2 = true;
+                e.SetUnwind ();
+            };
+
+            this.CheckAllKinds ((expected, expectedEx, fnTest) => {
+                visited1 = visited2 = false;
+                try {
+                    fnTest ();
+                    Assert.Fail ("TestRaiseContractMultipleHandlers2() exception not thrown");
+                } catch (Exception ex) {
+                    Assert.IsInstanceOfType (base.ContractExceptionType, ex, "TestRaiseContractMultipleHandlers2() wrong exception type");
+                    if (expectedEx == null) {
+                        Assert.IsNull (ex.InnerException, "TestRaiseContractMultipleHandlers2() inner exception not null");
+                    } else {
+                        Assert.AreSame (expectedEx, ex.InnerException, "TestRaiseContractMultipleHandlers2() wrong inner exception");
+                    }
+                    Assert.IsTrue (visited1, "TestRaiseContractMultipleHandlers2() handler 1 not visited");
+                    Assert.IsTrue (visited2, "TestRaiseContractMultipleHandlers2() handler 2 not visited");
+                }
+            });
+        }
+
+        /// <summary>
+        /// Both event handlers should be called. The exceptions are treated as calls to SetUnwind(), with
+        /// the exception being put in the InnerException.
+        /// </summary>
+        [Test, RunAgainstReference]
+        public void TestRaiseContractMultipleHandlers3 ()
+        {
+            bool visited1, visited2;
+            Contract.ContractFailed += (sender, e) => {
+                visited1 = true;
+                throw new InvalidOperationException ();
+            };
+            Contract.ContractFailed += (sender, e) => {
+                visited2 = true;
+                throw new InvalidOperationException ();
+            };
+
+            this.CheckAllKinds ((expected, expectedEx, fnTest) => {
+                visited1 = visited2 = false;
+                try {
+                    fnTest ();
+                    Assert.Fail ("TestRaiseContractMultipleHandlers3() failed to throw exception");
+                } catch (Exception ex) {
+                    Type expectedInnerExceptionType = expectedEx == null ? typeof (InvalidOperationException) : expectedEx.GetType ();
+                    Assert.IsInstanceOfType (base.ContractExceptionType, ex, "TestRaiseContractMultipleHandlers3() wrong exception type");
+                    Assert.IsInstanceOfType (expectedInnerExceptionType, ex.InnerException, "TestRaiseContractMultipleHandlers3() wrong inner exception type");
+                    Assert.IsTrue (visited1, "TestRaiseContractMultipleHandlers3() handler 1 not visited");
+                    Assert.IsTrue (visited2, "TestRaiseContractMultipleHandlers3() handler 2 not visited");
+                }
+            });
+        }
+
+        /// <summary>
+        /// Contract.TriggerFailure() triggers the assert. Check that the assert is triggered, with the correct text.
+        /// </summary>
+        [Test]
+        public void TestTriggerFailure ()
+        {
+            try {
+                ContractHelper.TriggerFailure (ContractFailureKind.Assert, "Display", null, "Condition", null);
+                Assert.Fail ("TestTriggerFailure() failed to throw exception");
+            } catch (Exception ex) {
+                Assert.IsInstanceOfType(typeof(NotImplementedException), ex, "TestTriggerFailure() wrong exception type");
+                //Assert.AreEqual ("Expression: Condition" + Environment.NewLine + "Description: Display", ex.Message, "TestTriggerFailure() wrong message");
+            }
+        }
+
+    }
+
+}
+#endif

Property changes on: Test/System.Diagnostics.Contracts/ContractHelperTest.cs
___________________________________________________________________
Added: svn:eol-style
   + native

Index: Test/System.Diagnostics.Contracts/Helpers/RunAgainstReferenceAttribute.cs
===================================================================
--- Test/System.Diagnostics.Contracts/Helpers/RunAgainstReferenceAttribute.cs	(revision 0)
+++ Test/System.Diagnostics.Contracts/Helpers/RunAgainstReferenceAttribute.cs	(revision 0)
@@ -0,0 +1,16 @@
+#if NET_4_0
+using System;
+using System.Collections.Generic;
+using System.Linq;
+using System.Text;
+using NUnit.Framework;
+
+namespace MonoTests.System.Diagnostics.Contracts.Helpers {
+
+    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
+    class RunAgainstReferenceAttribute : CategoryAttribute
+    {
+    }
+
+}
+#endif

Property changes on: Test/System.Diagnostics.Contracts/Helpers/RunAgainstReferenceAttribute.cs
___________________________________________________________________
Added: svn:eol-style
   + native

Index: Test/System.Diagnostics.Contracts/Helpers/TestContractBase.cs
===================================================================
--- Test/System.Diagnostics.Contracts/Helpers/TestContractBase.cs	(revision 0)
+++ Test/System.Diagnostics.Contracts/Helpers/TestContractBase.cs	(revision 0)
@@ -0,0 +1,46 @@
+#if NET_4_0
+using System;
+using System.Collections.Generic;
+using System.Linq;
+using System.Text;
+using NUnit.Framework;
+using System.Diagnostics.Contracts;
+using System.Diagnostics;
+using System.Reflection;
+using System.Diagnostics.Contracts.Internal;
+
+namespace MonoTests.System.Diagnostics.Contracts.Helpers {
+
+    public class TestContractBase {
+
+        protected TestContractBase() {
+            // Get the type of System.Diagnostics.Contracts.ContractException
+            // Have to do this differently depending on how the test is being run.
+            this.ContractExceptionType = Type.GetType("System.Diagnostics.Contracts.ContractException");
+            if (this.ContractExceptionType == null) {
+                // Special code for when Contracts namespace is not in CorLib
+                var mGetContractExceptionType = typeof (Contract).GetMethod ("GetContractExceptionType", BindingFlags.NonPublic | BindingFlags.Static);
+                this.ContractExceptionType = (Type) mGetContractExceptionType.Invoke (null, null);
+            }
+        }
+
+        [SetUp]
+        public void Setup() {
+            // Remove all event handlers from Contract.ContractFailed
+            var eventField = typeof(Contract).GetField("ContractFailed", BindingFlags.Static | BindingFlags.NonPublic);
+            if (eventField == null) {
+                // But in MS.NET it's done this way.
+                eventField = typeof(ContractHelper).GetField("contractFailedEvent", BindingFlags.Static | BindingFlags.NonPublic);
+            }
+            eventField.SetValue(null, null);
+        }
+
+        [TearDown]
+        public void TearDown() {
+        }
+
+        protected Type ContractExceptionType { get; private set; }
+
+    }
+}
+#endif

Property changes on: Test/System.Diagnostics.Contracts/Helpers/TestContractBase.cs
___________________________________________________________________
Added: svn:eol-style
   + native

Index: Test/System.Diagnostics.Contracts/ContractAssertTest.cs
===================================================================
--- Test/System.Diagnostics.Contracts/ContractAssertTest.cs	(revision 0)
+++ Test/System.Diagnostics.Contracts/ContractAssertTest.cs	(revision 0)
@@ -0,0 +1,173 @@
+#define CONTRACTS_FULL
+#define DEBUG
+
+#if NET_4_0
+using System;
+using System.Collections.Generic;
+using System.Linq;
+using System.Text;
+using NUnit.Framework;
+using System.Diagnostics.Contracts;
+using System.Diagnostics;
+using MonoTests.System.Diagnostics.Contracts.Helpers;
+
+namespace MonoTests.System.Diagnostics.Contracts {
+
+    [TestFixture]
+    public class ContractAssertTest : TestContractBase {
+
+        /// <summary>
+        /// Ensures that Assert(true) allows execution to continue.
+        /// </summary>
+        [Test, RunAgainstReference]
+        public void TestAssertTrue ()
+        {
+            Contract.Assert (true);
+        }
+
+        /// <summary>
+        /// Contract.Assert(false) will cause an assert to be triggered with the correct message.
+        /// </summary>
+        [Test]
+        public void TestAssertNoEventHandler ()
+        {
+            try {
+                Contract.Assert (false);
+                Assert.Fail ("TestAssertNoEventHandler() exception not thrown #1");
+            } catch (Exception ex) {
+                Assert.IsInstanceOfType (typeof (NotImplementedException), ex, "TestAssertNoEventHandler() wrong exception type #1");
+            }
+
+            try {
+                Contract.Assert (false, "Message");
+                Assert.Fail ("TestAssertNoEventHandler() exception not thrown #2");
+            } catch (Exception ex) {
+                Assert.IsInstanceOfType (typeof (NotImplementedException), ex, "TestAssertNoEventHandler() wrong exception type #2");
+            }
+        }
+
+        /// <summary>
+        /// Contract.Assert(true) will not call the ContractFailed event handler.
+        /// Contract.Assert(false) will call the ContractFailed event handler.
+        /// Because nothing is done in the event handler, an assert should be triggered.
+        /// </summary>
+        [Test]
+        public void TestAssertEventHandlerNoAction ()
+        {
+            bool visitedEventHandler = false;
+            Contract.ContractFailed += (sender, e) => {
+                visitedEventHandler = true;
+            };
+
+            Contract.Assert (true);
+
+            Assert.IsFalse (visitedEventHandler, "TestAssertEventHandlerNoAction() handler visited");
+
+            try {
+                Contract.Assert (false);
+                Assert.Fail ("TestAssertEventHandlerNoAction() exception not thrown");
+            } catch (Exception ex) {
+                Assert.IsInstanceOfType (typeof (NotImplementedException), ex, "TestAssertEventHandlerNoAction() wrong exception type");
+            }
+
+            Assert.IsTrue (visitedEventHandler, "TestAssertEventHandlerNoAction() handler not visited");
+        }
+
+        /// <summary>
+        /// Event handler calls SetHandled(), so no assert should be triggered.
+        /// </summary>
+        [Test, RunAgainstReference]
+        public void TestAssertEventHandlerSetHandled ()
+        {
+            Contract.ContractFailed += (sender, e) => {
+                e.SetHandled ();
+            };
+
+            Contract.Assert (false);
+        }
+
+        /// <summary>
+        /// Event handler calls SetUnwind(), so exception of type ContractException should be thrown.
+        /// </summary>
+        [Test, RunAgainstReference]
+        public void TestAssertEventHandlerSetUnwind ()
+        {
+            Contract.ContractFailed += (sender, e) => {
+                e.SetUnwind ();
+            };
+
+            try {
+                Contract.Assert (false);
+            } catch (Exception ex) {
+                Assert.IsInstanceOfType (base.ContractExceptionType, ex, "TestAssertEventHandlerSetUnwind() wrong exception type");
+                Assert.IsNull (ex.InnerException, "TestAssertEventHandlerSetUnwind() inner exception not null");
+            }
+        }
+
+        /// <summary>
+        /// Event handler calls SetHandled() and SetUnwind(), so exception of type ContractException should be thrown,
+        /// as SetUnwind overrides SetHandled.
+        /// </summary>
+        [Test, RunAgainstReference]
+        public void TestAssertEventHandlerSetUnwindHandled ()
+        {
+            Contract.ContractFailed += (sender, e) => {
+                e.SetHandled ();
+                e.SetUnwind ();
+            };
+
+            try {
+                Contract.Assert (false);
+            } catch (Exception ex) {
+                Assert.IsInstanceOfType (base.ContractExceptionType, ex, "TestAssertEventHandlerSetUnwindHandled() wrong exception type");
+                Assert.IsNull (ex.InnerException, "TestAssertEventHandlerSetUnwindHandled() inner exception not null");
+            }
+        }
+
+        /// <summary>
+        /// Event handler throws exception.
+        /// ContractException is thrown by Contract.Assert(), with InnerException set to the thrown exception.
+        /// </summary>
+        [Test, RunAgainstReference]
+        public void TestAssertEventHandlerThrows ()
+        {
+            Contract.ContractFailed += (sender, e) => {
+                throw new ArgumentNullException ();
+            };
+
+            try {
+                Contract.Assert (false);
+            } catch (Exception ex) {
+                Assert.IsInstanceOfType (base.ContractExceptionType, ex, "TestAssertEventHandlerSetUnwindHandled() wrong exception type");
+                Assert.IsInstanceOfType (typeof (ArgumentNullException), ex.InnerException, "TestAssertEventHandlerSetUnwindHandled() wrong inner exception type");
+            }
+        }
+
+        /// <summary>
+        /// Multiple event handlers are registered. Check that both are called, and that the SetHandled()
+        /// call in one of them is handled correctly.
+        /// </summary>
+        [Test, RunAgainstReference]
+        public void TestAssertMultipleHandlers ()
+        {
+            bool visited1 = false, visited2 = false;
+
+            Contract.ContractFailed += (sender, e) => {
+                visited1 = true;
+                Assert.IsFalse (e.Handled, "TestAssertMultipleHandlers() Handled incorrect #1");
+                e.SetHandled ();
+            };
+            Contract.ContractFailed += (sender, e) => {
+                visited2 = true;
+                Assert.IsTrue (e.Handled, "TestAssertMultipleHandlers() Handled incorrect #2");
+            };
+
+            Contract.Assert (false);
+
+            Assert.IsTrue (visited1, "TestAssertMultipleHandlers() visited1 incorrect");
+            Assert.IsTrue (visited2, "TestAssertMultipleHandlers() visited2 incorrect");
+        }
+
+    }
+}
+#endif

Property changes on: Test/System.Diagnostics.Contracts/ContractAssertTest.cs
___________________________________________________________________
Added: svn:eol-style
   + native

Index: Test/System.Diagnostics.Contracts/ContractMarkerMethodsTest.cs
===================================================================
--- Test/System.Diagnostics.Contracts/ContractMarkerMethodsTest.cs	(revision 0)
+++ Test/System.Diagnostics.Contracts/ContractMarkerMethodsTest.cs	(revision 0)
@@ -0,0 +1,74 @@
+#define CONTRACTS_FULL
+#define DEBUG
+
+#if NET_4_0
+using System;
+using System.Collections.Generic;
+using System.Linq;
+using System.Text;
+using NUnit.Framework;
+using System.Diagnostics.Contracts;
+using MonoTests.System.Diagnostics.Contracts.Helpers;
+
+namespace MonoTests.System.Diagnostics.Contracts {
+
+    [TestFixture]
+    public class ContractMarkerMethodsTest : TestContractBase {
+
+        /// <summary>
+        /// Contract.EndContractBlock() has no effects.
+        /// </summary>
+        [Test, RunAgainstReference]
+        public void TestEndContractBlock ()
+        {
+            Contract.EndContractBlock ();
+        }
+
+        /// <summary>
+        /// Contract.OldValue() has no effect, and always returns the default value for the type.
+        /// </summary>
+        [Test, RunAgainstReference]
+        public void TestOldValue ()
+        {
+            int i = Contract.OldValue<int> (8);
+            object o = Contract.OldValue<object> (new object ());
+
+            Assert.AreEqual (0, i, "TestOldValue() int value wrong");
+            Assert.IsNull (o, "TestOldValue() object value wrong");
+        }
+
+        /// <summary>
+        /// Contract.Result() has no effect, and always returns the default value for the type.
+        /// </summary>
+        [Test, RunAgainstReference]
+        public void TestResult ()
+        {
+            int i = Contract.Result<int> ();
+            object o = Contract.Result<object> ();
+
+            Assert.AreEqual (0, i, "TestResult() int value wrong");
+            Assert.IsNull (o, "TestResult() object value wrong");
+        }
+
+        /// <summary>
+        /// Contract.ValueAtReturn() has no effect, and always returns the default value for the type.
+        /// </summary>
+        [Test, RunAgainstReference]
+        public void TestValueAtReturn ()
+        {
+            int iOut, i;
+            object oOut, o;
+
+            i = Contract.ValueAtReturn (out iOut);
+            o = Contract.ValueAtReturn (out oOut);
+
+            Assert.AreEqual (0, i, "TestValueAtReturn() int return value wrong");
+            Assert.IsNull (o, "TestValueAtReturn() object return value wrong");
+            Assert.AreEqual (0, iOut, "TestValueAtReturn() int out value wrong");
+            Assert.IsNull (oOut, "TestValueAtReturn() object out value wrong");
+        }
+
+    }
+
+}
+#endif

Property changes on: Test/System.Diagnostics.Contracts/ContractMarkerMethodsTest.cs
___________________________________________________________________
Added: svn:eol-style
   + native

Index: Test/System.Diagnostics.Contracts/ContractMustUseRewriterTest.cs
===================================================================
--- Test/System.Diagnostics.Contracts/ContractMustUseRewriterTest.cs	(revision 0)
+++ Test/System.Diagnostics.Contracts/ContractMustUseRewriterTest.cs	(revision 0)
@@ -0,0 +1,119 @@
+#define CONTRACTS_FULL
+#define DEBUG
+
+#if NET_4_0
+using System;
+using System.Collections.Generic;
+using System.Linq;
+using System.Text;
+using NUnit.Framework;
+using System.Diagnostics.Contracts;
+using MonoTests.System.Diagnostics.Contracts.Helpers;
+
+namespace MonoTests.System.Diagnostics.Contracts {
+
+    [TestFixture]
+    public class ContractMustUseRewriterTest : TestContractBase {
+
+        private void CheckMustUseRewriter (string expectedMsg, params Action[] fnTests)
+        {
+            foreach (var fnTest in fnTests) {
+                try {
+                    fnTest ();
+                    Assert.Fail ("CheckMustUseRewriter() exception not thrown");
+                } catch (Exception ex) {
+                    Assert.IsInstanceOfType (typeof (NotImplementedException), ex, "CheckMustUseRewriter() wrong exception thrown");
+                }
+            }
+
+            bool handlerVisited = false;
+            Contract.ContractFailed += (sender, e) => {
+                handlerVisited = true;
+            };
+
+            foreach (var fnTest in fnTests) {
+                try {
+                    fnTest ();
+                    Assert.Fail ("CheckMustUseRewriter() exception not thrown");
+                } catch (Exception ex) {
+                    Assert.IsInstanceOfType (typeof (NotImplementedException), ex, "CheckMustUseRewriter() wrong exception thrown");
+                }
+            }
+
+            Assert.IsFalse (handlerVisited, "CheckMustUseRewriter() handled visited");
+        }
+
+        /// <summary>
+        /// Contract.Requires() ALWAYS triggers an assert, regardless of any other factors.
+        /// </summary>
+        [Test]
+        public void TestRequires ()
+        {
+            CheckMustUseRewriter ("Description: Must use the rewriter when using Contract.Requires",
+                () => Contract.Requires (true),
+                () => Contract.Requires (false),
+                () => Contract.Requires (true, "Message"),
+                () => Contract.Requires (false, "Message")
+            );
+        }
+
+        /// <summary>
+        /// Contract.Requires() ALWAYS triggers an assert, regardless of any other factors.
+        /// </summary>
+        [Test]
+        public void TestRequiresTException ()
+        {
+            CheckMustUseRewriter ("Description: Must use the rewriter when using Contract.Requires<TException>",
+                () => Contract.Requires<Exception> (true),
+                () => Contract.Requires<Exception> (false),
+                () => Contract.Requires<Exception> (true, "Message"),
+                () => Contract.Requires<Exception> (false, "Message")
+            );
+        }
+
+        /// <summary>
+        /// Contract.Ensures() ALWAYS triggers an assert, regardless of any other factors.
+        /// </summary>
+        [Test]
+        public void TestEnsures ()
+        {
+            CheckMustUseRewriter ("Description: Must use the rewriter when using Contract.Ensures",
+                () => Contract.Ensures (true),
+                () => Contract.Ensures (false),
+                () => Contract.Ensures (true, "Message"),
+                () => Contract.Ensures (false, "Message")
+            );
+        }
+
+        /// <summary>
+        /// Contract.Ensures() ALWAYS triggers an assert, regardless of any other factors.
+        /// </summary>
+        [Test]
+        public void TestEnsuresOnThrow ()
+        {
+            CheckMustUseRewriter ("Description: Must use the rewriter when using Contract.EnsuresOnThrow",
+                () => Contract.EnsuresOnThrow<Exception> (true),
+                () => Contract.EnsuresOnThrow<Exception> (false),
+                () => Contract.EnsuresOnThrow<Exception> (true, "Message"),
+                () => Contract.EnsuresOnThrow<Exception> (false, "Message")
+            );
+        }
+
+        /// <summary>
+        /// Contract.Ensures() ALWAYS triggers an assert, regardless of any other factors.
+        /// </summary>
+        [Test]
+        public void TestInvariant ()
+        {
+            CheckMustUseRewriter ("Description: Must use the rewriter when using Contract.Invariant",
+                () => Contract.Invariant (true),
+                () => Contract.Invariant (false),
+                () => Contract.Invariant (true, "Message"),
+                () => Contract.Invariant (false, "Message")
+            );
+        }
+
+    }
+
+}
+#endif

Property changes on: Test/System.Diagnostics.Contracts/ContractMustUseRewriterTest.cs
___________________________________________________________________
Added: svn:eol-style
   + native

