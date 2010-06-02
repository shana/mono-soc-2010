using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Diagnostics.Contracts.Internal {

	public static class ContractHelper {

		/// <summary>
		/// Behaviour defined in section 7.3 of UserDoc.pdf from Microsoft.
		/// Handlers called in turn, thrown exceptions acts as if SetUnwind() has been called.
		/// </summary>
		/// <param name="failureKind"></param>
		/// <param name="userMessage"></param>
		/// <param name="conditionText"></param>
		/// <param name="innerException"></param>
		/// <returns></returns>
		public static string RaiseContractFailedEvent(ContractFailureKind failureKind, string userMessage, string conditionText, Exception innerException) {

			StringBuilder msg = new StringBuilder(60);
			switch (failureKind) {
			case ContractFailureKind.Assert:
				msg.Append("Assertion failed");
				break;
			case ContractFailureKind.Assume:
				msg.Append("Assumption failed");
				break;
			case ContractFailureKind.Invariant:
				msg.Append("Invariant failed");
				break;
			case ContractFailureKind.Postcondition:
				msg.Append("Postcondition failed");
				break;
			case ContractFailureKind.PostconditionOnException:
				msg.Append("Postcondition failed after throwing an exception");
				break;
			case ContractFailureKind.Precondition:
				msg.Append("Precondition failed");
				break;
			default:
				throw new NotSupportedException("Not supported: " + failureKind);
			}
			if (conditionText != null) {
				msg.Append(": ");
				msg.Append(conditionText);
			} else {
				msg.Append('.');
			}
			if (userMessage != null) {
				msg.Append("  ");
				msg.Append(userMessage);
			}
			string msgString = msg.ToString();

			Exception handlerException = null;
			bool unwind = false, handled = false;

			var contractFailed = Contract.InternalContractFailedEvent;
			if (contractFailed != null) {
				// Execute all event handlers
				var handlers = contractFailed.GetInvocationList();
				var e = new ContractFailedEventArgs(failureKind, msgString, conditionText, innerException);
				foreach (var handler in handlers) {
					try {
						handler.DynamicInvoke(null, e);
					} catch (Exception ex) {
						e.SetUnwind();
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
				throw new ContractException(msgString, failureKind, conditionText, userMessage, ex);
			}

			return handled ? null : msgString;
		}

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

            Debug.Fail (msg.ToString ());
        }

	}

}
