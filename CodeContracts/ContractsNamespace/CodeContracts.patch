Index: corlib.dll.sources
===================================================================
--- corlib.dll.sources	(revision 158762)
+++ corlib.dll.sources	(working copy)
@@ -331,14 +331,17 @@
 System.Diagnostics.Contracts/Contract.cs
 System.Diagnostics.Contracts/ContractClassAttribute.cs
 System.Diagnostics.Contracts/ContractClassForAttribute.cs
+System.Diagnostics.Contracts/ContractException.cs
 System.Diagnostics.Contracts/ContractFailedEventArgs.cs
 System.Diagnostics.Contracts/ContractFailureKind.cs
 System.Diagnostics.Contracts/ContractInvariantMethodAttribute.cs
 System.Diagnostics.Contracts/ContractPublicPropertyNameAttribute.cs
 System.Diagnostics.Contracts/ContractReferenceAssemblyAttribute.cs
 System.Diagnostics.Contracts/ContractRuntimeIgnoredAttribute.cs
+System.Diagnostics.Contracts/ContractShouldAssertException.cs
 System.Diagnostics.Contracts/ContractVerificationAttribute.cs
 System.Diagnostics.Contracts/PureAttribute.cs
+System.Diagnostics.Contracts/Internal/ContractHelper.cs
 System.Diagnostics.SymbolStore/ISymbolBinder.cs
 System.Diagnostics.SymbolStore/ISymbolBinder1.cs
 System.Diagnostics.SymbolStore/ISymbolDocument.cs
Index: corlib_test.dll.sources
===================================================================
--- corlib_test.dll.sources	(revision 158762)
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

Index: System.Diagnostics.Contracts/Contract.cs
===================================================================
--- System.Diagnostics.Contracts/Contract.cs	(revision 158762)
+++ System.Diagnostics.Contracts/Contract.cs	(working copy)
@@ -3,8 +3,9 @@
 //
 // Authors:
 //    Miguel de Icaza (miguel@gnome.org)
+//    Chris Bacon (chrisbacon76@gmail.com)
 //
-// Copyright 2009 Novell (http://www.novell.com)
+// Copyright 2009, 2010 Novell (http://www.novell.com)
 //
 // Permission is hereby granted, free of charge, to any person obtaining
 // a copy of this software and associated documentation files (the
@@ -26,245 +27,433 @@
 // WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 //
 
-//
-// Things left to do:
-// 
-//   * This is a blind implementation from specs, without any testing, so the escalation
-//     is probably broken, and so are the messages and arguments to the eventargs properties
-//
-//   * How to plug the original condition into the Escalate method?   Perhaps we need
-//     some injection for it?
-//
-//   * The "originalException" in Escalate is nowhere used
-//
-//   * We do not Escalate everything that needs to be, perhaps that is the role of the
-//     rewriter to call Escalate with the proper values?
-//
-//   * I added a "new()" constraint to methods that took a TException because I needed
-//     to new the exception, but this is perhaps wrong.
-//
-//   * Result and ValueAtReturn, I need to check what these do in .NET 4, but have not
-//     installed it yet ;-)
-//
 using System;
 using System.Collections.Generic;
+using System.Diagnostics.Contracts.Internal;
 
 namespace System.Diagnostics.Contracts {
+
+    /// <summary>
+    /// Contains methods that allow various contracts to be specified in code.
+    /// </summary>
 #if NET_2_1 || NET_4_0
-	public
+    public
 #else
 	internal
 #endif
+ static class Contract {
 
-	static class Contract {
+        /// <summary>
+        /// Called on any contract failure.
+        /// </summary>
+        public static event EventHandler<ContractFailedEventArgs> ContractFailed;
 
-		public static event EventHandler<ContractFailedEventArgs> ContractFailed;
+        internal static EventHandler<ContractFailedEventArgs> InternalContractFailedEvent
+        {
+            get { return ContractFailed; }
+        }
 
-		static void Escalate (ContractFailureKind kind, Exception e, string text, params object [] args)
-		{
-			if (ContractFailed != null){
-				var ea = new ContractFailedEventArgs (kind, text, "<condition>", e ?? new Exception ());
-				ContractFailed (null, ea);
-				if (!ea.Unwind)
-					return;
-			}
-			Console.Error.WriteLine ("Contract.{0}: {1}", kind.ToString (), String.Format (text, args));
-		}
-		
-		[ConditionalAttribute("CONTRACTS_FULL")]
-		[ConditionalAttribute("DEBUG")]
-		public static void Assert (bool condition)
-		{
-			if (condition)
-				return;
-			Escalate (ContractFailureKind.Assert, null, "failure");
-		}
+        internal static Type GetContractExceptionType ()
+        {
+            return typeof (ContractException);
+        }
 
-		[ConditionalAttribute("DEBUG")]
-		[ConditionalAttribute("CONTRACTS_FULL")]
-		public static void Assert (bool condition, string userMessage)
-		{
-			if (condition)
-				return;
-			
-			Escalate (ContractFailureKind.Assert, null, userMessage);
-		}
+        internal static Type GetContractShouldAssertExceptionType ()
+        {
+            return typeof (ContractShouldAssertException);
+        }
 
-		[ConditionalAttribute("DEBUG")]
-		[ConditionalAttribute("CONTRACTS_FULL")]
-		public static void Assume(bool condition)
-		{
-			// At runtime, this behaves like assert
-			if (condition)
-				return;
+        static void ReportFailure (ContractFailureKind kind, string userMessage, string conditionText, Exception innerException)
+        {
+            string msg = ContractHelper.RaiseContractFailedEvent (kind, userMessage, conditionText, innerException);
+            // if msg is null, then it has been handled already, so don't do anything here
+            if (msg != null) {
+                ContractHelper.TriggerFailure (kind, msg, userMessage, conditionText, innerException);
+            }
+        }
 
-			Escalate (ContractFailureKind.Assume, null, "failure");
-		}
+        static void AssertMustUseRewriter (ContractFailureKind kind, string message)
+        {
+            if (Environment.UserInteractive) {
+                // FIXME: This should trigger an assertion.
+                // But code will never get here at the moment, as Environment.UserInteractive currently
+                // always returns false.
+                throw new ContractShouldAssertException (message);
+            } else {
+                // Note that FailFast() currently throws a NotImplemenetedException()
+#if NET_4_0
+                Environment.FailFast(message, new ExecutionEngineException());
+#else
+                Environment.FailFast(message);
+#endif
+            }
+        }
 
-		[ConditionalAttribute("CONTRACTS_FULL")]
-		[ConditionalAttribute("DEBUG")]
-		public static void Assume (bool condition, string userMessage)
-		{
-			if (condition)
-				return;
-			
-			Escalate (ContractFailureKind.Assume, null, userMessage);
-		}
+        /// <summary>
+        /// Trigger a contract failure if condition is false.
+        /// </summary>
+        /// <param name="condition">The condition to verify.</param>
+        [ConditionalAttribute ("DEBUG")]
+        [ConditionalAttribute ("CONTRACTS_FULL")]
+        public static void Assert (bool condition)
+        {
+            if (condition)
+                return;
 
-		[ConditionalAttribute("CONTRACTS_FULL")]
-		public static void EndContractBlock ()
-		{
-			// seems to be some kind of flag, no code generated
-		}
+            ReportFailure (ContractFailureKind.Assert, null, null, null);
+        }
 
-		[ConditionalAttribute("CONTRACTS_FULL")]
-		public static void Ensures (bool condition)
-		{
-			// Requires binary rewriter to work
-		}
+        /// <summary>
+        /// Trigger a contract failure if condition is false.
+        /// </summary>
+        /// <param name="condition">The condition to verify.</param>
+        /// <param name="userMessage">Message to display if condition is false.</param>
+        [ConditionalAttribute ("DEBUG")]
+        [ConditionalAttribute ("CONTRACTS_FULL")]
+        public static void Assert (bool condition, string userMessage)
+        {
+            if (condition)
+                return;
 
-		[ConditionalAttribute("CONTRACTS_FULL")]
-		public static void Ensures (bool condition, string userMessage)
-		{
-			// Requires binary rewriter to work
-		}
+            ReportFailure (ContractFailureKind.Assert, userMessage, null, null);
+        }
 
-		[ConditionalAttribute("CONTRACTS_FULL")]
-		public static void EnsuresOnThrow<TException> (bool condition) where TException : Exception
-		{
-			// Requires binary rewriter to work
-		}
+        /// <summary>
+        /// Forces the condition specified to be considered true by the code contract tools.
+        /// </summary>
+        /// <param name="condition">The condition that is assumed to be true.</param>
+        /// <remarks>
+        /// If the code contracts are included at runtime, this acts like a Contract.Assert().
+        /// </remarks>
+        [ConditionalAttribute ("DEBUG")]
+        [ConditionalAttribute ("CONTRACTS_FULL")]
+        public static void Assume (bool condition)
+        {
+            // At runtime, this behaves like assert
+            if (condition)
+                return;
 
-		[ConditionalAttribute("CONTRACTS_FULL")]
-		public static void EnsuresOnThrow<TException> (bool condition, string userMessage) where TException : Exception
-		{
-			// Requires binary rewriter to work
-		}
+            ReportFailure (ContractFailureKind.Assume, null, null, null);
+        }
 
-		public static bool Exists<T> (IEnumerable<T> collection, Predicate<T> predicate)
-		{
-			if (predicate == null)
-				throw new ArgumentNullException ("predicate");
-			if (collection == null)
-				throw new ArgumentNullException ("collection");
-			
-			foreach (var t in collection)
-				if (predicate (t))
-					return true;
-			return false;
-		}
+        /// <summary>
+        /// Forces the condition specified to be considered true by the code contract tools.
+        /// </summary>
+        /// <param name="condition">The condition that is assumed to be true.</param>
+        /// <param name="userMessage">Message to display if condition is false.</param>
+        /// <remarks>
+        /// If the code contracts are included at runtime, this acts like a Contract.Assert().
+        /// </remarks>
+        [ConditionalAttribute ("DEBUG")]
+        [ConditionalAttribute ("CONTRACTS_FULL")]
+        public static void Assume (bool condition, string userMessage)
+        {
+            // At runtime, this behaves like assert
+            if (condition)
+                return;
 
-		public static bool Exists (int fromInclusive, int toExclusive, Predicate<int> predicate)
-		{
-			if (predicate == null)
-				throw new ArgumentNullException ("predicate");
-			if (toExclusive < fromInclusive)
-				throw new ArgumentException ("toExclusitve < fromInclusive");
-			
-			for (int i = fromInclusive; i < toExclusive; i++)
-				if (predicate (i))
-					return true;
+            ReportFailure (ContractFailureKind.Assume, userMessage, null, null);
+        }
 
-			return false;
-		}
+        /// <summary>
+        /// Marker method to mark the end of the legacy requires code block.
+        /// </summary>
+        /// <remarks>
+        /// Legacy requires are preconditions that perform tests and throw exceptions.
+        /// This is the only form of legacy require allowed.
+        /// </remarks>
+        [ConditionalAttribute ("CONTRACTS_FULL")]
+        public static void EndContractBlock ()
+        {
+            // Marker method, no code required.
+        }
 
-		public static bool ForAll<T> (IEnumerable<T> collection, Predicate<T> predicate)
-		{
-			if (predicate == null)
-				throw new ArgumentNullException ("predicate");
-			if (collection == null)
-				throw new ArgumentNullException ("collection");
-			
-			foreach (var t in collection)
-				if (!predicate (t))
-					return false;
+        /// <summary>
+        /// Specifies a postconditon contract for normal return.
+        /// </summary>
+        /// <param name="condition">The postcondition to verify.</param>
+        /// <remarks>
+        /// All code contract method calls must be at the beginning of the method or property.
+        /// The code contract rewriter tool must be used to enable contracts to be verified at runtime.
+        /// </remarks>
+        [ConditionalAttribute ("CONTRACTS_FULL")]
+        public static void Ensures (bool condition)
+        {
+            AssertMustUseRewriter (ContractFailureKind.Postcondition, "Contract.Ensures");
+        }
 
-			return true;
-		}
+        /// <summary>
+        /// Specifies a postconditon contract for normal return.
+        /// </summary>
+        /// <param name="condition">The postcondition to verify.</param>
+        /// <param name="userMessage">Message to display on contract failure.</param>
+        /// <remarks>
+        /// All code contract method calls must be at the beginning of the method or property.
+        /// The code contract rewriter tool must be used to enable contracts to be verified at runtime.
+        /// </remarks>
+        [ConditionalAttribute ("CONTRACTS_FULL")]
+        public static void Ensures (bool condition, string userMessage)
+        {
+            AssertMustUseRewriter (ContractFailureKind.Postcondition, "Contract.Ensures");
+        }
 
-		public static bool ForAll (int fromInclusive, int toExclusive, Predicate<int> predicate)
-		{
-			if (predicate == null)
-				throw new ArgumentNullException ("predicate");
-			if (toExclusive < fromInclusive)
-				throw new ArgumentException ("toExclusitve < fromInclusive");
-			
-			for (int i = fromInclusive; i < toExclusive; i++)
-				if (!predicate (i))
-					return false;
+        /// <summary>
+        /// Specifies a postcondition contract for when an exception is thrown.
+        /// </summary>
+        /// <typeparam name="TException">The exception type that causes this postcondition to be verified.</typeparam>
+        /// <param name="condition">The postcondition to verify.</param>
+        /// <remarks>
+        /// All code contract method calls must be at the beginning of the method or property.
+        /// The code contract rewriter tool must be used to enable contracts to be verified at runtime.
+        /// </remarks>
+        [ConditionalAttribute ("CONTRACTS_FULL")]
+        public static void EnsuresOnThrow<TException> (bool condition) where TException : Exception
+        {
+            AssertMustUseRewriter (ContractFailureKind.Postcondition, "Contract.EnsuresOnThrow");
+        }
 
-			return true;
-		}
+        /// <summary>
+        /// Specifies a postcondition contract for when an exception is thrown.
+        /// </summary>
+        /// <typeparam name="TException">The exception type that causes this postcondition to be verified.</typeparam>
+        /// <param name="condition">The postcondition to verify.</param>
+        /// <param name="userMessage">Message to display on contract failure.</param>
+        /// <remarks>
+        /// All code contract method calls must be at the beginning of the method or property.
+        /// The code contract rewriter tool must be used to enable contracts to be verified at runtime.
+        /// </remarks>
+        [ConditionalAttribute ("CONTRACTS_FULL")]
+        public static void EnsuresOnThrow<TException> (bool condition, string userMessage) where TException : Exception
+        {
+            AssertMustUseRewriter (ContractFailureKind.Postcondition, "Contract.EnsuresOnThrow");
+        }
 
-		[ConditionalAttribute("CONTRACTS_FULL")]
-		public static void Invariant (bool condition)
-		{
-			// Binary rewriter required
-		}
+        /// <summary>
+        /// Determines that at least one element in the collection satisfies the predicate.
+        /// </summary>
+        /// <typeparam name="T">The type of elements in the collection.</typeparam>
+        /// <param name="collection">The collection to check.</param>
+        /// <param name="predicate">The predicate to apply to each element of the collection.</param>
+        /// <returns>Whether at least one element in the collection satisfies the predicate.</returns>
+        /// <remarks>
+        /// This method can be used within a contract condition to allow contracts to be applied to collections.
+        /// </remarks>
+        public static bool Exists<T> (IEnumerable<T> collection, Predicate<T> predicate)
+        {
+            if (predicate == null)
+                throw new ArgumentNullException ("predicate");
+            if (collection == null)
+                throw new ArgumentNullException ("collection");
 
-		[ConditionalAttribute("CONTRACTS_FULL")]
-		public static void Invariant (bool condition, string userMessage)
-		{
-			// Binary rewriter required
-		}
+            foreach (var t in collection)
+                if (predicate (t))
+                    return true;
+            return false;
+        }
 
-		static Exception RewriterRequired ()
-		{
-			return new Exception ("The rewriter is required to use this method");
-		}
-	
-		public static T OldValue<T> (T value)
-		{
-			// This is really the binary rewriter that should kick-in
-			throw RewriterRequired ();
-		}
+        /// <summary>
+        /// Determines that at least one value in the range satisfies the predicate.
+        /// </summary>
+        /// <param name="fromInclusive">The inclusive start of the range.</param>
+        /// <param name="toExclusive">The exclusive end of the range.</param>
+        /// <param name="predicate">The predicate to apply to each value within the range.</param>
+        /// <returns>Whether at least one value in the range satisfies the predicate.</returns>
+        /// <remarks>
+        /// This method can be used within a contract condition to allow contracts to be applied to integer ranges.
+        /// </remarks>
+        public static bool Exists (int fromInclusive, int toExclusive, Predicate<int> predicate)
+        {
+            if (predicate == null)
+                throw new ArgumentNullException ("predicate");
+            if (toExclusive < fromInclusive)
+                throw new ArgumentException ("toExclusitve < fromInclusive");
 
-		[ConditionalAttribute("CONTRACTS_FULL")]
-		[MonoTODO ("Currently throws Exception, needs to throw the proper exception")]
-		public static void Requires (bool condition)
-		{
-			if (!condition){
-				Escalate (ContractFailureKind.Precondition, null, "failure");
-				throw new Exception ();
-			}
-		}
+            for (int i = fromInclusive; i < toExclusive; i++)
+                if (predicate (i))
+                    return true;
 
-		[ConditionalAttribute("CONTRACTS_FULL")]
-		[MonoTODO ("Currently throws Exception, needs to throw the proper exception")]
-		public static void Requires (bool condition, string userMessage)
-		{
-			if (!condition){
-				Escalate (ContractFailureKind.Precondition, null, userMessage);
-				throw new Exception ();
-			}
-		}
+            return false;
+        }
 
-		public static void Requires<TException> (bool condition) where TException : Exception, new ()
-		{
-			if (!condition){
-				var e = new TException ();
-				Escalate (ContractFailureKind.Precondition, e, "failure");
-				throw e;
-			}
-		}
+        /// <summary>
+        /// Determines that all elements in the collection satisfy the predicate.
+        /// </summary>
+        /// <typeparam name="T">The type of elements in the collection.</typeparam>
+        /// <param name="collection">The collection to check.</param>
+        /// <param name="predicate">The predicate to apply to each element of the collection.</param>
+        /// <returns>Whether all elements in the collection satisfy the predicate.</returns>
+        /// <remarks>
+        /// This method can be used within a contract condition to allow contracts to be applied to collections.
+        /// </remarks>
+        public static bool ForAll<T> (IEnumerable<T> collection, Predicate<T> predicate)
+        {
+            if (predicate == null)
+                throw new ArgumentNullException ("predicate");
+            if (collection == null)
+                throw new ArgumentNullException ("collection");
 
-		public static void Requires<TException>(bool condition, string userMessage) where TException : Exception, new ()
-		{
-			if (!condition){
-				var e = new TException ();
-				Escalate (ContractFailureKind.Precondition, e, userMessage);
-				throw e;
-			}
-		}
+            foreach (var t in collection)
+                if (!predicate (t))
+                    return false;
 
-		public static T Result<T> ()
-		{
-			throw RewriterRequired ();
-		}
+            return true;
+        }
 
-		public static T ValueAtReturn<T> (out T value)
-		{
-			throw RewriterRequired ();
-		}
-	}
+        /// <summary>
+        /// Determines that all values in the range satisfy the predicate.
+        /// </summary>
+        /// <param name="fromInclusive">The inclusive start of the range.</param>
+        /// <param name="toExclusive">The exclusive end of the range.</param>
+        /// <param name="predicate">The predicate to apply to each value within the range.</param>
+        /// <returns>Whether all values in the range satisfy the predicate.</returns>
+        /// <remarks>
+        /// This method can be used within a contract condition to allow contracts to be applied to integer ranges.
+        /// </remarks>
+        public static bool ForAll (int fromInclusive, int toExclusive, Predicate<int> predicate)
+        {
+            if (predicate == null)
+                throw new ArgumentNullException ("predicate");
+            if (toExclusive < fromInclusive)
+                throw new ArgumentException ("toExclusitve < fromInclusive");
+
+            for (int i = fromInclusive; i < toExclusive; i++)
+                if (!predicate (i))
+                    return false;
+
+            return true;
+        }
+
+        /// <summary>
+        /// Specifies an invariant class-level condition.
+        /// </summary>
+        /// <param name="condition">The invariant condition to verify.</param>
+        /// <remarks>
+        /// Invariant conditions can only be used in a method marked with the ContractInvariantMethod attribute.
+        /// The code contract rewriter tool must be used to enable contracts to be verified at runtime.
+        /// </remarks>
+        [ConditionalAttribute ("CONTRACTS_FULL")]
+        public static void Invariant (bool condition)
+        {
+            AssertMustUseRewriter (ContractFailureKind.Invariant, "Contract.Invariant");
+        }
+
+        /// <summary>
+        /// Specifies an invariant class-level condition.
+        /// </summary>
+        /// <param name="condition">The invariant condition to verify.</param>
+        /// <param name="userMessage">Message to display on contract failure.</param>
+        [ConditionalAttribute ("CONTRACTS_FULL")]
+        public static void Invariant (bool condition, string userMessage)
+        {
+            AssertMustUseRewriter (ContractFailureKind.Invariant, "Contract.Invariant");
+        }
+
+        /// <summary>
+        /// Used within a postcondition to represent an initial value.
+        /// </summary>
+        /// <typeparam name="T">The type of the value.</typeparam>
+        /// <param name="value">The field or parameter of which to represent the initial value.</param>
+        /// <returns>The initial value.</returns>
+        /// <remarks>
+        /// The code contract rewriter tool must be used for this method to be effective.
+        /// Otherwise the default value for the type is returned.
+        /// </remarks>
+        public static T OldValue<T> (T value)
+        {
+            // Marker method, no code required.
+            return default (T);
+        }
+
+        /// <summary>
+        /// Specifies a precondition contract for normal return.
+        /// </summary>
+        /// <param name="condition">The precondition to verify.</param>
+        /// <remarks>
+        /// All code contract method calls must be at the beginning of the method or property.
+        /// The code contract rewriter tool must be used to enable contracts to be verified at runtime.
+        /// </remarks>
+        [ConditionalAttribute ("CONTRACTS_FULL")]
+        public static void Requires (bool condition)
+        {
+            AssertMustUseRewriter (ContractFailureKind.Precondition, "Contract.Requires");
+        }
+
+        /// <summary>
+        /// Specifies a precondition contract for normal return.
+        /// </summary>
+        /// <param name="condition">The precondition to verify.</param>
+        /// <param name="userMessage">Message to display on contract failure.</param>
+        /// <remarks>
+        /// All code contract method calls must be at the beginning of the method or property.
+        /// The code contract rewriter tool must be used to enable contracts to be verified at runtime.
+        /// </remarks>
+        [ConditionalAttribute ("CONTRACTS_FULL")]
+        public static void Requires (bool condition, string userMessage)
+        {
+            AssertMustUseRewriter (ContractFailureKind.Precondition, "Contract.Requires");
+        }
+
+        /// <summary>
+        /// Specifies a precondition contract for when an exception is thrown.
+        /// </summary>
+        /// <typeparam name="TException">The exception type that causes this precondition to be verified.</typeparam>
+        /// <param name="condition">The precondition to verify.</param>
+        /// <remarks>
+        /// All code contract method calls must be at the beginning of the method or property.
+        /// The code contract rewriter tool must be used to enable contracts to be verified at runtime.
+        /// </remarks>
+        public static void Requires<TException> (bool condition) where TException : Exception
+        {
+            AssertMustUseRewriter (ContractFailureKind.Precondition, "Contract.Requires<TException>");
+        }
+
+        /// <summary>
+        /// Specifies a precondition contract for when an exception is thrown.
+        /// </summary>
+        /// <typeparam name="TException">The exception type that causes this precondition to be verified.</typeparam>
+        /// <param name="condition">The precondition to verify.</param>
+        /// <param name="userMessage">Message to display on contract failure.</param>
+        /// <remarks>
+        /// All code contract method calls must be at the beginning of the method or property.
+        /// The code contract rewriter tool must be used to enable contracts to be verified at runtime.
+        /// </remarks>
+        public static void Requires<TException> (bool condition, string userMessage) where TException : Exception
+        {
+            AssertMustUseRewriter (ContractFailureKind.Precondition, "Contract.Requires<TException>");
+        }
+
+        /// <summary>
+        /// Used within a postcondition to represent the return value.
+        /// </summary>
+        /// <typeparam name="T">The type of the return value.</typeparam>
+        /// <returns>The return value.</returns>
+        /// <remarks>
+        /// The code contract rewriter tool must be used for this method to be effective.
+        /// Otherwise the default value for the type is returned.
+        /// </remarks>
+        public static T Result<T> ()
+        {
+            // Marker method, no code required.
+            return default (T);
+        }
+
+        /// <summary>
+        /// Used within a postcondition to represent the final value of an out parameter.
+        /// </summary>
+        /// <typeparam name="T">The type of the out parameter.</typeparam>
+        /// <param name="value">The out parameter.</param>
+        /// <returns>The final value of the out parameter.</returns>
+        /// <remarks>
+        /// The code contract rewriter tool must be used for this method to be effective.
+        /// Otherwise the default value for the type is returned in the out parameter and the return value.
+        /// </remarks>
+        public static T ValueAtReturn<T> (out T value)
+        {
+            // Marker method, no code required.
+            value = default (T);
+            return default (T);
+        }
+
+    }
+
 }

Property changes on: System.Diagnostics.Contracts/Contract.cs
___________________________________________________________________
Added: svn:eol-style
   + native

Index: System.Diagnostics.Contracts/ContractRuntimeIgnoredAttribute.cs
===================================================================
--- System.Diagnostics.Contracts/ContractRuntimeIgnoredAttribute.cs	(revision 158762)
+++ System.Diagnostics.Contracts/ContractRuntimeIgnoredAttribute.cs	(working copy)
@@ -28,11 +28,16 @@
 #if NET_2_1 || NET_4_0
 using System;
 namespace System.Diagnostics.Contracts {
-	[Conditional ("CONTRACTS_FULL")]
-	[AttributeUsage (AttributeTargets.Method | AttributeTargets.Property, AllowMultiple=false, Inherited = true)]
-	public sealed class ContractRuntimeIgnoredAttribute : Attribute {
-		public ContractRuntimeIgnoredAttribute() {}
-	}
+
+    /// <summary>
+    /// Marks a method or property as having no runtime behaviour when called
+    /// in methods in System.Diagnostics.Contracts.Contract.
+    /// </summary>
+    [Conditional ("CONTRACTS_FULL")]
+    [AttributeUsage (AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
+    public sealed class ContractRuntimeIgnoredAttribute : Attribute {
+        public ContractRuntimeIgnoredAttribute () { }
+    }
 }
 #endif
 

Property changes on: System.Diagnostics.Contracts/ContractRuntimeIgnoredAttribute.cs
___________________________________________________________________
Added: svn:eol-style
   + native

Index: System.Diagnostics.Contracts/ContractInvariantMethodAttribute.cs
===================================================================
--- System.Diagnostics.Contracts/ContractInvariantMethodAttribute.cs	(revision 158762)
+++ System.Diagnostics.Contracts/ContractInvariantMethodAttribute.cs	(working copy)
@@ -28,13 +28,20 @@
 #if NET_2_1 || NET_4_0
 using System;
 namespace System.Diagnostics.Contracts {
-	[Conditional ("CONTRACTS_FULL")]
-	[AttributeUsage (AttributeTargets.Method, Inherited=false)]
-	public sealed class ContractInvariantMethodAttribute : Attribute {
-		public ContractInvariantMethodAttribute ()
-		{
-		}
-	}
+
+    /// <summary>
+    /// Specifies that this method contains Contract invariant conditions for this class.
+    /// </summary>
+    /// <remarks>
+    /// There may be multiple methods with this attribute in one class, their effect is accumulated.
+    /// The method must take no parameters and return void, and may be any visibility.
+    /// </remarks>
+    [Conditional ("CONTRACTS_FULL")]
+    [AttributeUsage (AttributeTargets.Method, Inherited = false)]
+    public sealed class ContractInvariantMethodAttribute : Attribute {
+        public ContractInvariantMethodAttribute ()
+        {
+        }
+    }
 }
 #endif
-

Property changes on: System.Diagnostics.Contracts/ContractInvariantMethodAttribute.cs
___________________________________________________________________
Added: svn:eol-style
   + native

Index: System.Diagnostics.Contracts/ContractReferenceAssemblyAttribute.cs
===================================================================
--- System.Diagnostics.Contracts/ContractReferenceAssemblyAttribute.cs	(revision 158762)
+++ System.Diagnostics.Contracts/ContractReferenceAssemblyAttribute.cs	(working copy)
@@ -28,10 +28,15 @@
 #if NET_2_1 || NET_4_0
 using System;
 namespace System.Diagnostics.Contracts {
-	[AttributeUsage (AttributeTargets.Assembly)]
-	public sealed class ContractReferenceAssemblyAttribute : Attribute {
-		public ContractReferenceAssemblyAttribute () {}
-	}
+
+    /// <summary>
+    /// Marks an assembly as being a contarct reference assembly.
+    /// This assembly will contain no code except as required by the contracts.
+    /// </summary>
+    [AttributeUsage (AttributeTargets.Assembly)]
+    public sealed class ContractReferenceAssemblyAttribute : Attribute {
+        public ContractReferenceAssemblyAttribute () { }
+    }
 }
 #endif
 

Property changes on: System.Diagnostics.Contracts/ContractReferenceAssemblyAttribute.cs
___________________________________________________________________
Added: svn:eol-style
   + native

Index: System.Diagnostics.Contracts/ChangeLog
===================================================================
--- System.Diagnostics.Contracts/ChangeLog	(revision 158762)
+++ System.Diagnostics.Contracts/ChangeLog	(working copy)
@@ -1,3 +1,10 @@
+2010-06-07  Chris Bacon <chrisbacon76@gmail.com>
+
+	* Contract.cs: Implemented correct rutnime behaviour withour rewriter.
+	* ContractException.cs: New file
+	* ContractShouldAssertException.cs: New file. Used to simulate Asserts because
+	  Mono doesn't have a way of asserting from within corlib.
+
 2010-03-11  Sebastien Pouliot  <sebastien@ximian.com>
 
 	* *.cs: Add NET_2_1 since contracts are part of SL4 :-)
Index: System.Diagnostics.Contracts/ContractClassAttribute.cs
===================================================================
--- System.Diagnostics.Contracts/ContractClassAttribute.cs	(revision 158762)
+++ System.Diagnostics.Contracts/ContractClassAttribute.cs	(working copy)
@@ -28,20 +28,25 @@
 #if NET_2_1 || NET_4_0
 using System;
 namespace System.Diagnostics.Contracts {
-	[Conditional ("CONTRACTS_FULL")]
-	[Conditional ("DEBUG")]
-	[AttributeUsage (AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited=false)]
-	public sealed class ContractClassAttribute : Attribute {
-		Type type;
 
-		public ContractClassAttribute (Type typeContainingContracts)
-		{
-			type = typeContainingContracts;
-		}
+    /// <summary>
+    /// Marks this type as having another class that contains its code contracts.
+    /// </summary>
+    [Conditional ("CONTRACTS_FULL")]
+    [Conditional ("DEBUG")]
+    [AttributeUsage (AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false)]
+    public sealed class ContractClassAttribute : Attribute {
+        Type type;
 
-		public Type TypeContainingContracts {
-			get { return type; }
-		}
-	}
+        public ContractClassAttribute (Type typeContainingContracts)
+        {
+            type = typeContainingContracts;
+        }
+
+        public Type TypeContainingContracts
+        {
+            get { return type; }
+        }
+    }
 }
 #endif

Property changes on: System.Diagnostics.Contracts/ContractClassAttribute.cs
___________________________________________________________________
Added: svn:eol-style
   + native

Index: System.Diagnostics.Contracts/ContractFailureKind.cs
===================================================================
--- System.Diagnostics.Contracts/ContractFailureKind.cs	(revision 158762)
+++ System.Diagnostics.Contracts/ContractFailureKind.cs	(working copy)
@@ -30,18 +30,20 @@
 
 namespace System.Diagnostics.Contracts {
 
+    /// <summary>
+    /// The different kinds of contracts that can fail.
+    /// </summary>
 #if NET_2_1 || NET_4_0
-	public
+    public
 #else
 	internal
 #endif
-
-	enum ContractFailureKind {
-		Precondition,
-		Postcondition,
-		PostconditionOnException,
-		Invariant,
-		Assert,
-		Assume
-	}
+    enum ContractFailureKind {
+        Precondition,
+        Postcondition,
+        PostconditionOnException,
+        Invariant,
+        Assert,
+        Assume
+    }
 }

Property changes on: System.Diagnostics.Contracts/ContractFailureKind.cs
___________________________________________________________________
Added: svn:eol-style
   + native

Index: System.Diagnostics.Contracts/ContractException.cs
===================================================================
--- System.Diagnostics.Contracts/ContractException.cs	(revision 0)
+++ System.Diagnostics.Contracts/ContractException.cs	(revision 0)
@@ -0,0 +1,54 @@
+﻿//
+// System.Diagnostics.Contracts.ContractException.cs
+//
+// Authors:
+//    Chris Bacon (chrisbacon76@gmail.com)
+//
+// Copyright 2010 Novell (http://www.novell.com)
+//
+// Permission is hereby granted, free of charge, to any person obtaining
+// a copy of this software and associated documentation files (the
+// "Software"), to deal in the Software without restriction, including
+// without limitation the rights to use, copy, modify, merge, publish,
+// distribute, sublicense, and/or sell copies of the Software, and to
+// permit persons to whom the Software is furnished to do so, subject to
+// the following conditions:
+// 
+// The above copyright notice and this permission notice shall be
+// included in all copies or substantial portions of the Software.
+// 
+// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
+// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
+// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
+// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
+// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
+// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
+// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
+//
+
+using System;
+
+namespace System.Diagnostics.Contracts {
+
+    class ContractException : Exception {
+
+        internal ContractException (string failure, ContractFailureKind kind, string condition, string userMessage, Exception innerException)
+            : base (failure, innerException)
+        {
+            this.Failure = failure;
+            this.Kind = kind;
+            this.Condition = condition;
+            this.UserMessage = userMessage;
+        }
+
+        public string Failure { get; private set; }
+
+        public ContractFailureKind Kind { get; private set; }
+
+        public string Condition { get; private set; }
+
+        public string UserMessage { get; private set; }
+
+    }
+
+}

Property changes on: System.Diagnostics.Contracts/ContractException.cs
___________________________________________________________________
Added: svn:eol-style
   + native

Index: System.Diagnostics.Contracts/ContractShouldAssertException.cs
===================================================================
--- System.Diagnostics.Contracts/ContractShouldAssertException.cs	(revision 0)
+++ System.Diagnostics.Contracts/ContractShouldAssertException.cs	(revision 0)
@@ -0,0 +1,42 @@
+﻿//
+// System.Diagnostics.Contracts.ContractException.cs
+//
+// Authors:
+//    Chris Bacon (chrisbacon76@gmail.com)
+//
+// Copyright 2010 Novell (http://www.novell.com)
+//
+// Permission is hereby granted, free of charge, to any person obtaining
+// a copy of this software and associated documentation files (the
+// "Software"), to deal in the Software without restriction, including
+// without limitation the rights to use, copy, modify, merge, publish,
+// distribute, sublicense, and/or sell copies of the Software, and to
+// permit persons to whom the Software is furnished to do so, subject to
+// the following conditions:
+// 
+// The above copyright notice and this permission notice shall be
+// included in all copies or substantial portions of the Software.
+// 
+// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
+// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
+// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
+// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
+// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
+// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
+// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
+//
+
+using System;
+
+namespace System.Diagnostics.Contracts {
+
+    class ContractShouldAssertException : Exception {
+
+        public ContractShouldAssertException (string msg)
+            : base (msg)
+        {
+        }
+
+    }
+
+}

Property changes on: System.Diagnostics.Contracts/ContractShouldAssertException.cs
___________________________________________________________________
Added: svn:eol-style
   + native

Index: System.Diagnostics.Contracts/ContractClassForAttribute.cs
===================================================================
--- System.Diagnostics.Contracts/ContractClassForAttribute.cs	(revision 158762)
+++ System.Diagnostics.Contracts/ContractClassForAttribute.cs	(working copy)
@@ -28,19 +28,24 @@
 #if NET_2_1 || NET_4_0
 using System;
 namespace System.Diagnostics.Contracts {
-	[Conditional ("CONTRACTS_FULL")]
-	[AttributeUsage (AttributeTargets.Delegate | AttributeTargets.Interface | AttributeTargets.Class, Inherited=false)]
-	public sealed class ContractClassForAttribute : Attribute {
-		Type type;
 
-		public ContractClassForAttribute (Type typeContractsAreFor)
-		{
-			type = typeContractsAreFor;
-		}
+    /// <summary>
+    /// Marks a class as containing contracts for the type specified.
+    /// </summary>
+    [Conditional ("CONTRACTS_FULL")]
+    [AttributeUsage (AttributeTargets.Class, Inherited = false)]
+    public sealed class ContractClassForAttribute : Attribute {
+        Type type;
 
-		public Type TypeContractsAreFor {
-			get { return type; }
-		}
-	}
+        public ContractClassForAttribute (Type typeContractsAreFor)
+        {
+            type = typeContractsAreFor;
+        }
+
+        public Type TypeContractsAreFor
+        {
+            get { return type; }
+        }
+    }
 }
 #endif

Property changes on: System.Diagnostics.Contracts/ContractClassForAttribute.cs
___________________________________________________________________
Added: svn:eol-style
   + native

Index: System.Diagnostics.Contracts/PureAttribute.cs
===================================================================
--- System.Diagnostics.Contracts/PureAttribute.cs	(revision 158762)
+++ System.Diagnostics.Contracts/PureAttribute.cs	(working copy)
@@ -30,13 +30,15 @@
 
 using System;
 
-namespace System.Diagnostics.Contracts
-{
-	[ConditionalAttribute("CONTRACTS_FULL")]
-	[AttributeUsageAttribute (AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Parameter | AttributeTargets.Delegate)]
-	public sealed class PureAttribute : Attribute
-	{
-	}
+namespace System.Diagnostics.Contracts {
+
+    /// <summary>
+    /// Marks a method or type to indicate that it has no side effects.
+    /// </summary>
+    [ConditionalAttribute ("CONTRACTS_FULL")]
+    [AttributeUsageAttribute (AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Parameter | AttributeTargets.Delegate)]
+    public sealed class PureAttribute : Attribute {
+    }
 }
 
 #endif
Index: System.Diagnostics.Contracts/ContractFailedEventArgs.cs
===================================================================
--- System.Diagnostics.Contracts/ContractFailedEventArgs.cs	(revision 158762)
+++ System.Diagnostics.Contracts/ContractFailedEventArgs.cs	(working copy)
@@ -3,6 +3,7 @@
 //
 // Authors:
 //    Miguel de Icaza (miguel@gnome.org)
+//    Chris Bacon (chrisbacon76@gmail.com)
 //
 // Copyright 2009 Novell (http://www.novell.com)
 //
@@ -30,39 +31,70 @@
 
 namespace System.Diagnostics.Contracts {
 
+    /// <summary>
+    /// Contains event data for the Contract.ContractFailed event.
+    /// </summary>
 #if NET_2_1 || NET_4_0
-	public
+    public
 #else
 	internal
 #endif
-	sealed class ContractFailedEventArgs : EventArgs {
-		
-		public ContractFailedEventArgs (ContractFailureKind failureKind, string message, string condition, Exception originalException)
-		{
-			Condition = condition;
-			FailureKind = failureKind;
-			Handled = false;
-			Unwind = true;
-			Message = message;
-			OriginalException = originalException;
-		}
+    sealed class ContractFailedEventArgs : EventArgs {
 
-		public void SetHandled ()
-		{
-			Handled = true;
-		}
+        public ContractFailedEventArgs (ContractFailureKind failureKind, string message, string condition, Exception originalException)
+        {
+            Condition = condition;
+            FailureKind = failureKind;
+            Handled = false;
+            Unwind = false; // MS docs are incorrect - this should default to false.
+            Message = message;
+            OriginalException = originalException;
+        }
 
-		public void SetUnwind ()
-		{
-			Unwind = true;
-		}
+        /// <summary>
+        /// Mark this contract failure as having been handled.
+        /// </summary>
+        public void SetHandled ()
+        {
+            Handled = true;
+        }
 
-		public string Condition { get; private set; }
-		public ContractFailureKind FailureKind { get; private set; }
-		public bool Handled { get; private set; }
-		public bool Unwind { get; private set; }
-		public string Message { get; private set; }
-		public Exception OriginalException { get; private set; }
-	}
+        /// <summary>
+        /// Request that this contract failure unwind the stack by throwing an exception.
+        /// </summary>
+        public void SetUnwind ()
+        {
+            Unwind = true;
+        }
+
+        /// <summary>
+        /// Gets the condition that caused the contract failure.
+        /// </summary>
+        public string Condition { get; private set; }
+
+        /// <summary>
+        /// Gets the kind of contract failure.
+        /// </summary>
+        public ContractFailureKind FailureKind { get; private set; }
+
+        /// <summary>
+        /// Gets whether the contract failure has been handled.
+        /// </summary>
+        public bool Handled { get; private set; }
+
+        /// <summary>
+        /// Gets whether the contract failure should unwind the stack.
+        /// </summary>
+        public bool Unwind { get; private set; }
+
+        /// <summary>
+        /// Gets the message associated with this contract failure.
+        /// </summary>
+        public string Message { get; private set; }
+
+        /// <summary>
+        /// Gets the original exception that caused this contract failure, if any.
+        /// </summary>
+        public Exception OriginalException { get; private set; }
+    }
 }
-

Property changes on: System.Diagnostics.Contracts/ContractFailedEventArgs.cs
___________________________________________________________________
Added: svn:eol-style
   + native

Index: System.Diagnostics.Contracts/ContractVerificationAttribute.cs
===================================================================
--- System.Diagnostics.Contracts/ContractVerificationAttribute.cs	(revision 158762)
+++ System.Diagnostics.Contracts/ContractVerificationAttribute.cs	(working copy)
@@ -28,20 +28,29 @@
 #if NET_2_1 || NET_4_0
 using System;
 namespace System.Diagnostics.Contracts {
-	[Conditional ("CONTRACTS_FULL")]
-	[AttributeUsageAttribute (AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property)]
-	public sealed class ContractVerificationAttribute : Attribute {
-		bool val;	
 
-		public ContractVerificationAttribute (bool value)
-		{
-			val = value;
-		}
+    /// <summary>
+    /// Indicates whether the code contract verification tools should verify this item.
+    /// If not, the item is assumed to be correct.
+    /// </summary>
+    [Conditional ("CONTRACTS_FULL")]
+    [AttributeUsageAttribute (AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property)]
+    public sealed class ContractVerificationAttribute : Attribute {
+        bool val;
 
-		public bool Value {
-			get { return val; }
-		}
-	}
+        public ContractVerificationAttribute (bool value)
+        {
+            val = value;
+        }
+
+        /// <summary>
+        /// Gets whether to include this item in verification.
+        /// </summary>
+        public bool Value
+        {
+            get { return val; }
+        }
+    }
 }
 #endif
 

Property changes on: System.Diagnostics.Contracts/ContractVerificationAttribute.cs
___________________________________________________________________
Added: svn:eol-style
   + native

Index: System.Diagnostics.Contracts/ContractPublicPropertyNameAttribute.cs
===================================================================
--- System.Diagnostics.Contracts/ContractPublicPropertyNameAttribute.cs	(revision 158762)
+++ System.Diagnostics.Contracts/ContractPublicPropertyNameAttribute.cs	(working copy)
@@ -28,19 +28,25 @@
 #if NET_2_1 || NET_4_0
 using System;
 namespace System.Diagnostics.Contracts {
-	[Conditional ("CONTRACTS_FULL")]
-	[AttributeUsage (AttributeTargets.Field)]
-	public sealed class ContractPublicPropertyNameAttribute : Attribute {
-		string name;
 
-		public ContractPublicPropertyNameAttribute (string name)
-		{
-			this.name = name;
-		}
+    /// <summary>
+    /// Specifies a public property name associated with a field, allowing a less-visible field to be used
+    /// in contracts within a method.
+    /// </summary>
+    [Conditional ("CONTRACTS_FULL")]
+    [AttributeUsage (AttributeTargets.Field)]
+    public sealed class ContractPublicPropertyNameAttribute : Attribute {
+        string name;
 
-		public string Name {
-			get { return name; }
-		}
-	}
+        public ContractPublicPropertyNameAttribute (string name)
+        {
+            this.name = name;
+        }
+
+        public string Name
+        {
+            get { return name; }
+        }
+    }
 }
 #endif

Property changes on: System.Diagnostics.Contracts/ContractPublicPropertyNameAttribute.cs
___________________________________________________________________
Added: svn:eol-style
   + native

Index: System.Diagnostics.Contracts/Internal/ContractHelper.cs
===================================================================
--- System.Diagnostics.Contracts/Internal/ContractHelper.cs	(revision 0)
+++ System.Diagnostics.Contracts/Internal/ContractHelper.cs	(revision 0)
@@ -0,0 +1,165 @@
+//
+// System.Diagnostics.Contracts.Internal.ContractHelper.cs
+//
+// Authors:
+//    Chris Bacon (chrisbacon76@gmail.com)
+//
+// Copyright 2010 Novell (http://www.novell.com)
+//
+// Permission is hereby granted, free of charge, to any person obtaining
+// a copy of this software and associated documentation files (the
+// "Software"), to deal in the Software without restriction, including
+// without limitation the rights to use, copy, modify, merge, publish,
+// distribute, sublicense, and/or sell copies of the Software, and to
+// permit persons to whom the Software is furnished to do so, subject to
+// the following conditions:
+// 
+// The above copyright notice and this permission notice shall be
+// included in all copies or substantial portions of the Software.
+// 
+// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
+// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
+// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
+// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
+// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
+// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
+// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
+//
+
+using System;
+using System.Text;
+
+namespace System.Diagnostics.Contracts.Internal {
+
+    /// <summary>
+    /// Implements methods required by the contract rewriter to handle contract failures.
+    /// </summary>
+#if NET_2_1 || NET_4_0
+    public
+#else
+	internal
+#endif
+    static class ContractHelper {
+
+        /// <summary>
+        /// Behaviour defined in section 7.3 of UserDoc.pdf from Microsoft.
+        /// Determines the default contract failure behaviour, which is that
+        /// event handlers are called in turn, thrown exceptions acts as if SetUnwind() has been called.
+        /// </summary>
+        /// <param name="failureKind">The kind of contract failure.</param>
+        /// <param name="userMessage">The user message describing this contract failure.</param>
+        /// <param name="conditionText">The condition that caused this contract failure.</param>
+        /// <param name="innerException">The exception that caused this contract filure, if any.</param>
+        /// <returns></returns>
+        public static string RaiseContractFailedEvent (ContractFailureKind failureKind, string userMessage, string conditionText, Exception innerException)
+        {
+
+            StringBuilder msg = new StringBuilder (60);
+            switch (failureKind) {
+            case ContractFailureKind.Assert:
+                msg.Append ("Assertion failed");
+                break;
+            case ContractFailureKind.Assume:
+                msg.Append ("Assumption failed");
+                break;
+            case ContractFailureKind.Invariant:
+                msg.Append ("Invariant failed");
+                break;
+            case ContractFailureKind.Postcondition:
+                msg.Append ("Postcondition failed");
+                break;
+            case ContractFailureKind.PostconditionOnException:
+                msg.Append ("Postcondition failed after throwing an exception");
+                break;
+            case ContractFailureKind.Precondition:
+                msg.Append ("Precondition failed");
+                break;
+            default:
+                throw new NotSupportedException ("Not supported: " + failureKind);
+            }
+            if (conditionText != null) {
+                msg.Append (": ");
+                msg.Append (conditionText);
+            } else {
+                msg.Append ('.');
+            }
+            if (userMessage != null) {
+                msg.Append ("  ");
+                msg.Append (userMessage);
+            }
+            string msgString = msg.ToString ();
+
+            Exception handlerException = null;
+            bool unwind = false, handled = false;
+
+            var contractFailed = Contract.InternalContractFailedEvent;
+            if (contractFailed != null) {
+                // Execute all event handlers
+                var handlers = contractFailed.GetInvocationList ();
+                var e = new ContractFailedEventArgs (failureKind, msgString, conditionText, innerException);
+                foreach (var handler in handlers) {
+                    try {
+                        handler.DynamicInvoke (null, e);
+                    } catch (Exception ex) {
+                        e.SetUnwind ();
+                        // If multiple handlers throw an exception then the specification states that it
+                        // is undetermined which one becomes the InnerException.
+                        handlerException = ex.InnerException;
+                    }
+                }
+                unwind = e.Unwind;
+                handled = e.Handled;
+            }
+
+            if (unwind) {
+                Exception ex = innerException ?? handlerException;
+                throw new ContractException (msgString, failureKind, conditionText, userMessage, ex);
+            }
+
+            return handled ? null : msgString;
+        }
+
+        /// <summary>
+        /// Implements the default failure behaviour.
+        /// </summary>
+        /// <param name="kind">The kind of contract failure.</param>
+        /// <param name="displayMessage">The message to display describing this contract failure.</param>
+        /// <param name="userMessage">The user message describing this contract failure.</param>
+        /// <param name="conditionText">The condition that caused this contract failure.</param>
+        /// <param name="innerException">The exception that caused this contract filure, if any.</param>
+        public static void TriggerFailure (ContractFailureKind kind, string displayMessage, string userMessage, string conditionText, Exception innerException)
+        {
+            StringBuilder msg = new StringBuilder (50);
+
+            if (conditionText != null) {
+                msg.Append ("Expression: ");
+                msg.AppendLine (conditionText);
+            }
+            msg.Append ("Description: ");
+            if (displayMessage != null) {
+                msg.Append (displayMessage);
+            }
+
+            // FIXME: This should trigger an assertion, but don't know now to do this
+            // in corlib, without using System. So throw exception instead.
+            //Debug.Fail (msg.ToString ());
+
+            if (Environment.UserInteractive) {
+                // FIXME: This should trigger an assertion.
+                // But code will never get here at the moment, as Environment.UserInteractive currently
+                // always returns false.
+                throw new ContractShouldAssertException (msg.ToString ());
+            } else {
+                // Note that FailFast() currently throws a NotImplemenetedException()
+#if NET_4_0
+                Environment.FailFast(msg.ToString(), new ExecutionEngineException());
+#else
+                Environment.FailFast(msg.ToString());
+#endif
+            }
+
+        }
+
+    }
+
+}

Property changes on: System.Diagnostics.Contracts/Internal/ContractHelper.cs
___________________________________________________________________
Added: svn:eol-style
   + native

Index: System.Diagnostics.Contracts/Internal/ChangeLog
===================================================================
--- System.Diagnostics.Contracts/Internal/ChangeLog	(revision 0)
+++ System.Diagnostics.Contracts/Internal/ChangeLog	(revision 0)
@@ -0,0 +1,3 @@
+2010-06-07  Chris Bacon <chrisbacon76@gmail.com>
+
+	* ContractHelper.cs: New file
\ No newline at end of file

Property changes on: System.Diagnostics.Contracts/Internal/ChangeLog
___________________________________________________________________
Added: svn:eol-style
   + native

