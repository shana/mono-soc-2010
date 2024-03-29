//
// System.Diagnostics.Contracts.Internal.ContractHelper.cs
//
// Authors:
//    Chris Bacon (chrisbacon76@gmail.com)
//
// Copyright 2010 Novell (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Text;

namespace System.Diagnostics.Contracts.Internal {

    /// <summary>
    /// Implements methods required by the contract rewriter to handle contract failures.
    /// </summary>
#if NET_2_1 || NET_4_0
    public
#else
	internal
#endif
    static class ContractHelper {

        /// <summary>
        /// Behaviour defined in section 7.3 of UserDoc.pdf from Microsoft.
        /// Determines the default contract failure behaviour, which is that
        /// event handlers are called in turn, thrown exceptions acts as if SetUnwind() has been called.
        /// </summary>
        /// <param name="failureKind">The kind of contract failure.</param>
        /// <param name="userMessage">The user message describing this contract failure.</param>
        /// <param name="conditionText">The condition that caused this contract failure.</param>
        /// <param name="innerException">The exception that caused this contract filure, if any.</param>
        /// <returns></returns>
        public static string RaiseContractFailedEvent (ContractFailureKind failureKind, string userMessage, string conditionText, Exception innerException)
        {

            StringBuilder msg = new StringBuilder (60);
            switch (failureKind) {
            case ContractFailureKind.Assert:
                msg.Append ("Assertion failed");
                break;
            case ContractFailureKind.Assume:
                msg.Append ("Assumption failed");
                break;
            case ContractFailureKind.Invariant:
                msg.Append ("Invariant failed");
                break;
            case ContractFailureKind.Postcondition:
                msg.Append ("Postcondition failed");
                break;
            case ContractFailureKind.PostconditionOnException:
                msg.Append ("Postcondition failed after throwing an exception");
                break;
            case ContractFailureKind.Precondition:
                msg.Append ("Precondition failed");
                break;
            default:
                throw new NotSupportedException ("Not supported: " + failureKind);
            }
            if (conditionText != null) {
                msg.Append (": ");
                msg.Append (conditionText);
            } else {
                msg.Append ('.');
            }
            if (userMessage != null) {
                msg.Append ("  ");
                msg.Append (userMessage);
            }
            string msgString = msg.ToString ();

            Exception handlerException = null;
            bool unwind = false, handled = false;

            var contractFailed = Contract.InternalContractFailedEvent;
            if (contractFailed != null) {
                // Execute all event handlers
                var handlers = contractFailed.GetInvocationList ();
                var e = new ContractFailedEventArgs (failureKind, msgString, conditionText, innerException);
                foreach (var handler in handlers) {
                    try {
                        handler.DynamicInvoke (null, e);
                    } catch (Exception ex) {
                        e.SetUnwind ();
                        // If multiple handlers throw an exception then the specification states that it
                        // is undetermined which one becomes the InnerException.
                        handlerException = ex.InnerException;
                    }
                }
                unwind = e.Unwind;
                handled = e.Handled;
            }

            if (unwind) {
                Exception ex = innerException ?? handlerException;
                throw new ContractException (msgString, failureKind, conditionText, userMessage, ex);
            }

            return handled ? null : msgString;
        }

        /// <summary>
        /// Implements the default failure behaviour.
        /// </summary>
        /// <param name="kind">The kind of contract failure.</param>
        /// <param name="displayMessage">The message to display describing this contract failure.</param>
        /// <param name="userMessage">The user message describing this contract failure.</param>
        /// <param name="conditionText">The condition that caused this contract failure.</param>
        /// <param name="innerException">The exception that caused this contract filure, if any.</param>
        public static void TriggerFailure (ContractFailureKind kind, string displayMessage, string userMessage, string conditionText, Exception innerException)
        {
            StringBuilder msg = new StringBuilder (50);

            if (conditionText != null) {
                msg.Append ("Expression: ");
                msg.AppendLine (conditionText);
            }
            msg.Append ("Description: ");
            if (displayMessage != null) {
                msg.Append (displayMessage);
            }

            // FIXME: This should trigger an assertion, but don't know now to do this
            // in corlib, without using System. So throw exception instead.
            //Debug.Fail (msg.ToString ());

            if (Environment.UserInteractive) {
                // FIXME: This should trigger an assertion.
                // But code will never get here at the moment, as Environment.UserInteractive currently
                // always returns false.
                throw new ContractShouldAssertException (msg.ToString ());
            } else {
                // Note that FailFast() currently throws a NotImplemenetedException()
#if NET_4_0
                Environment.FailFast(msg.ToString(), new ExecutionEngineException());
#else
                Environment.FailFast(msg.ToString());
#endif
            }

        }

    }

}
