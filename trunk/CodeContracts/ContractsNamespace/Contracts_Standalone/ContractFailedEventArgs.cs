//
// System.Diagnostics.Contracts.ContractFailedEventArgs.cs
//
// Authors:
//    Miguel de Icaza (miguel@gnome.org)
//
// Copyright 2009 Novell (http://www.novell.com)
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

namespace System.Diagnostics.Contracts {

    /// <summary>
    /// Contains event data for the Contract.ContractFailed event.
    /// </summary>
#if NET_2_1 || NET_4_0
    public
#else
	internal
#endif
 sealed class ContractFailedEventArgs : EventArgs {

        public ContractFailedEventArgs (ContractFailureKind failureKind, string message, string condition, Exception originalException)
        {
            Condition = condition;
            FailureKind = failureKind;
            Handled = false;
            Unwind = false; // MS docs are incorrect - this should default to false.
            Message = message;
            OriginalException = originalException;
        }

        /// <summary>
        /// Mark this contract failure as having been handled.
        /// </summary>
        public void SetHandled ()
        {
            Handled = true;
        }

        /// <summary>
        /// Request that this contract failure unwind the stack by throwing an exception.
        /// </summary>
        public void SetUnwind ()
        {
            Unwind = true;
        }

        /// <summary>
        /// Gets the condition that caused the contract failure.
        /// </summary>
        public string Condition { get; private set; }

        /// <summary>
        /// Gets the kind of contract failure.
        /// </summary>
        public ContractFailureKind FailureKind { get; private set; }

        /// <summary>
        /// Gets whether the contract failure has been handled.
        /// </summary>
        public bool Handled { get; private set; }

        /// <summary>
        /// Gets whether the contract failure should unwind the stack.
        /// </summary>
        public bool Unwind { get; private set; }

        /// <summary>
        /// Gets the message associated with this contract failure.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Gets the original exception that caused this contract failure, if any.
        /// </summary>
        public Exception OriginalException { get; private set; }
    }
}
