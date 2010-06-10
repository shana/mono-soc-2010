//
// System.Diagnostics.Contracts.Contract.cs
//
// Authors:
//    Miguel de Icaza (miguel@gnome.org)
//    Chris Bacon (chrisbacon76@gmail.com)
//
// Copyright 2009, 2010 Novell (http://www.novell.com)
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
using System.Collections.Generic;
using System.Diagnostics.Contracts.Internal;

namespace System.Diagnostics.Contracts {

    /// <summary>
    /// Contains methods that allow various contracts to be specified in code.
    /// </summary>
#if NET_2_1 || NET_4_0
    public
#else
	internal
#endif
 static class Contract {

        /// <summary>
        /// Called on any contract failure.
        /// </summary>
        public static event EventHandler<ContractFailedEventArgs> ContractFailed;

        internal static EventHandler<ContractFailedEventArgs> InternalContractFailedEvent
        {
            get { return ContractFailed; }
        }

        internal static Type GetContractExceptionType ()
        {
            return typeof (ContractException);
        }

        internal static Type GetContractShouldAssertExceptionType ()
        {
            return typeof (ContractShouldAssertException);
        }

        static void ReportFailure (ContractFailureKind kind, string userMessage, string conditionText, Exception innerException)
        {
            string msg = ContractHelper.RaiseContractFailedEvent (kind, userMessage, conditionText, innerException);
            // if msg is null, then it has been handled already, so don't do anything here
            if (msg != null) {
                ContractHelper.TriggerFailure (kind, msg, userMessage, conditionText, innerException);
            }
        }

        static void AssertMustUseRewriter (ContractFailureKind kind, string message)
        {
            if (Environment.UserInteractive) {
                // FIXME: This should trigger an assertion.
                // But code will never get here at the moment, as Environment.UserInteractive currently
                // always returns false.
                throw new ContractShouldAssertException (message);
            } else {
                // Note that FailFast() currently throws a NotImplemenetedException()
#if NET_4_0
                Environment.FailFast(message, new ExecutionEngineException());
#else
                Environment.FailFast(message);
#endif
            }
        }

        /// <summary>
        /// Trigger a contract failure if condition is false.
        /// </summary>
        /// <param name="condition">The condition to verify.</param>
        [ConditionalAttribute ("DEBUG")]
        [ConditionalAttribute ("CONTRACTS_FULL")]
        public static void Assert (bool condition)
        {
            if (condition)
                return;

            ReportFailure (ContractFailureKind.Assert, null, null, null);
        }

        /// <summary>
        /// Trigger a contract failure if condition is false.
        /// </summary>
        /// <param name="condition">The condition to verify.</param>
        /// <param name="userMessage">Message to display if condition is false.</param>
        [ConditionalAttribute ("DEBUG")]
        [ConditionalAttribute ("CONTRACTS_FULL")]
        public static void Assert (bool condition, string userMessage)
        {
            if (condition)
                return;

            ReportFailure (ContractFailureKind.Assert, userMessage, null, null);
        }

        /// <summary>
        /// Forces the condition specified to be considered true by the code contract tools.
        /// </summary>
        /// <param name="condition">The condition that is assumed to be true.</param>
        /// <remarks>
        /// If the code contracts are included at runtime, this acts like a Contract.Assert().
        /// </remarks>
        [ConditionalAttribute ("DEBUG")]
        [ConditionalAttribute ("CONTRACTS_FULL")]
        public static void Assume (bool condition)
        {
            // At runtime, this behaves like assert
            if (condition)
                return;

            ReportFailure (ContractFailureKind.Assume, null, null, null);
        }

        /// <summary>
        /// Forces the condition specified to be considered true by the code contract tools.
        /// </summary>
        /// <param name="condition">The condition that is assumed to be true.</param>
        /// <param name="userMessage">Message to display if condition is false.</param>
        /// <remarks>
        /// If the code contracts are included at runtime, this acts like a Contract.Assert().
        /// </remarks>
        [ConditionalAttribute ("DEBUG")]
        [ConditionalAttribute ("CONTRACTS_FULL")]
        public static void Assume (bool condition, string userMessage)
        {
            // At runtime, this behaves like assert
            if (condition)
                return;

            ReportFailure (ContractFailureKind.Assume, userMessage, null, null);
        }

        /// <summary>
        /// Marker method to mark the end of the legacy requires code block.
        /// </summary>
        /// <remarks>
        /// Legacy requires are preconditions that perform tests and throw exceptions.
        /// This is the only form of legacy require allowed.
        /// </remarks>
        [ConditionalAttribute ("CONTRACTS_FULL")]
        public static void EndContractBlock ()
        {
            // Marker method, no code required.
        }

        /// <summary>
        /// Specifies a postconditon contract for normal return.
        /// </summary>
        /// <param name="condition">The postcondition to verify.</param>
        /// <remarks>
        /// All code contract method calls must be at the beginning of the method or property.
        /// The code contract rewriter tool must be used to enable contracts to be verified at runtime.
        /// </remarks>
        [ConditionalAttribute ("CONTRACTS_FULL")]
        public static void Ensures (bool condition)
        {
            AssertMustUseRewriter (ContractFailureKind.Postcondition, "Contract.Ensures");
        }

        /// <summary>
        /// Specifies a postconditon contract for normal return.
        /// </summary>
        /// <param name="condition">The postcondition to verify.</param>
        /// <param name="userMessage">Message to display on contract failure.</param>
        /// <remarks>
        /// All code contract method calls must be at the beginning of the method or property.
        /// The code contract rewriter tool must be used to enable contracts to be verified at runtime.
        /// </remarks>
        [ConditionalAttribute ("CONTRACTS_FULL")]
        public static void Ensures (bool condition, string userMessage)
        {
            AssertMustUseRewriter (ContractFailureKind.Postcondition, "Contract.Ensures");
        }

        /// <summary>
        /// Specifies a postcondition contract for when an exception is thrown.
        /// </summary>
        /// <typeparam name="TException">The exception type that causes this postcondition to be verified.</typeparam>
        /// <param name="condition">The postcondition to verify.</param>
        /// <remarks>
        /// All code contract method calls must be at the beginning of the method or property.
        /// The code contract rewriter tool must be used to enable contracts to be verified at runtime.
        /// </remarks>
        [ConditionalAttribute ("CONTRACTS_FULL")]
        public static void EnsuresOnThrow<TException> (bool condition) where TException : Exception
        {
            AssertMustUseRewriter (ContractFailureKind.Postcondition, "Contract.EnsuresOnThrow");
        }

        /// <summary>
        /// Specifies a postcondition contract for when an exception is thrown.
        /// </summary>
        /// <typeparam name="TException">The exception type that causes this postcondition to be verified.</typeparam>
        /// <param name="condition">The postcondition to verify.</param>
        /// <param name="userMessage">Message to display on contract failure.</param>
        /// <remarks>
        /// All code contract method calls must be at the beginning of the method or property.
        /// The code contract rewriter tool must be used to enable contracts to be verified at runtime.
        /// </remarks>
        [ConditionalAttribute ("CONTRACTS_FULL")]
        public static void EnsuresOnThrow<TException> (bool condition, string userMessage) where TException : Exception
        {
            AssertMustUseRewriter (ContractFailureKind.Postcondition, "Contract.EnsuresOnThrow");
        }

        /// <summary>
        /// Determines that at least one element in the collection satisfies the predicate.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection to check.</param>
        /// <param name="predicate">The predicate to apply to each element of the collection.</param>
        /// <returns>Whether at least one element in the collection satisfies the predicate.</returns>
        /// <remarks>
        /// This method can be used within a contract condition to allow contracts to be applied to collections.
        /// </remarks>
        public static bool Exists<T> (IEnumerable<T> collection, Predicate<T> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException ("predicate");
            if (collection == null)
                throw new ArgumentNullException ("collection");

            foreach (var t in collection)
                if (predicate (t))
                    return true;
            return false;
        }

        /// <summary>
        /// Determines that at least one value in the range satisfies the predicate.
        /// </summary>
        /// <param name="fromInclusive">The inclusive start of the range.</param>
        /// <param name="toExclusive">The exclusive end of the range.</param>
        /// <param name="predicate">The predicate to apply to each value within the range.</param>
        /// <returns>Whether at least one value in the range satisfies the predicate.</returns>
        /// <remarks>
        /// This method can be used within a contract condition to allow contracts to be applied to integer ranges.
        /// </remarks>
        public static bool Exists (int fromInclusive, int toExclusive, Predicate<int> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException ("predicate");
            if (toExclusive < fromInclusive)
                throw new ArgumentException ("toExclusitve < fromInclusive");

            for (int i = fromInclusive; i < toExclusive; i++)
                if (predicate (i))
                    return true;

            return false;
        }

        /// <summary>
        /// Determines that all elements in the collection satisfy the predicate.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection to check.</param>
        /// <param name="predicate">The predicate to apply to each element of the collection.</param>
        /// <returns>Whether all elements in the collection satisfy the predicate.</returns>
        /// <remarks>
        /// This method can be used within a contract condition to allow contracts to be applied to collections.
        /// </remarks>
        public static bool ForAll<T> (IEnumerable<T> collection, Predicate<T> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException ("predicate");
            if (collection == null)
                throw new ArgumentNullException ("collection");

            foreach (var t in collection)
                if (!predicate (t))
                    return false;

            return true;
        }

        /// <summary>
        /// Determines that all values in the range satisfy the predicate.
        /// </summary>
        /// <param name="fromInclusive">The inclusive start of the range.</param>
        /// <param name="toExclusive">The exclusive end of the range.</param>
        /// <param name="predicate">The predicate to apply to each value within the range.</param>
        /// <returns>Whether all values in the range satisfy the predicate.</returns>
        /// <remarks>
        /// This method can be used within a contract condition to allow contracts to be applied to integer ranges.
        /// </remarks>
        public static bool ForAll (int fromInclusive, int toExclusive, Predicate<int> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException ("predicate");
            if (toExclusive < fromInclusive)
                throw new ArgumentException ("toExclusitve < fromInclusive");

            for (int i = fromInclusive; i < toExclusive; i++)
                if (!predicate (i))
                    return false;

            return true;
        }

        /// <summary>
        /// Specifies an invariant class-level condition.
        /// </summary>
        /// <param name="condition">The invariant condition to verify.</param>
        /// <remarks>
        /// Invariant conditions can only be used in a method marked with the ContractInvariantMethod attribute.
        /// The code contract rewriter tool must be used to enable contracts to be verified at runtime.
        /// </remarks>
        [ConditionalAttribute ("CONTRACTS_FULL")]
        public static void Invariant (bool condition)
        {
            AssertMustUseRewriter (ContractFailureKind.Invariant, "Contract.Invariant");
        }

        /// <summary>
        /// Specifies an invariant class-level condition.
        /// </summary>
        /// <param name="condition">The invariant condition to verify.</param>
        /// <param name="userMessage">Message to display on contract failure.</param>
        [ConditionalAttribute ("CONTRACTS_FULL")]
        public static void Invariant (bool condition, string userMessage)
        {
            AssertMustUseRewriter (ContractFailureKind.Invariant, "Contract.Invariant");
        }

        /// <summary>
        /// Used within a postcondition to represent an initial value.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The field or parameter of which to represent the initial value.</param>
        /// <returns>The initial value.</returns>
        /// <remarks>
        /// The code contract rewriter tool must be used for this method to be effective.
        /// Otherwise the default value for the type is returned.
        /// </remarks>
        public static T OldValue<T> (T value)
        {
            // Marker method, no code required.
            return default (T);
        }

        /// <summary>
        /// Specifies a precondition contract for normal return.
        /// </summary>
        /// <param name="condition">The precondition to verify.</param>
        /// <remarks>
        /// All code contract method calls must be at the beginning of the method or property.
        /// The code contract rewriter tool must be used to enable contracts to be verified at runtime.
        /// </remarks>
        [ConditionalAttribute ("CONTRACTS_FULL")]
        public static void Requires (bool condition)
        {
            AssertMustUseRewriter (ContractFailureKind.Precondition, "Contract.Requires");
        }

        /// <summary>
        /// Specifies a precondition contract for normal return.
        /// </summary>
        /// <param name="condition">The precondition to verify.</param>
        /// <param name="userMessage">Message to display on contract failure.</param>
        /// <remarks>
        /// All code contract method calls must be at the beginning of the method or property.
        /// The code contract rewriter tool must be used to enable contracts to be verified at runtime.
        /// </remarks>
        [ConditionalAttribute ("CONTRACTS_FULL")]
        public static void Requires (bool condition, string userMessage)
        {
            AssertMustUseRewriter (ContractFailureKind.Precondition, "Contract.Requires");
        }

        /// <summary>
        /// Specifies a precondition contract for when an exception is thrown.
        /// </summary>
        /// <typeparam name="TException">The exception type that causes this precondition to be verified.</typeparam>
        /// <param name="condition">The precondition to verify.</param>
        /// <remarks>
        /// All code contract method calls must be at the beginning of the method or property.
        /// The code contract rewriter tool must be used to enable contracts to be verified at runtime.
        /// </remarks>
        public static void Requires<TException> (bool condition) where TException : Exception
        {
            AssertMustUseRewriter (ContractFailureKind.Precondition, "Contract.Requires<TException>");
        }

        /// <summary>
        /// Specifies a precondition contract for when an exception is thrown.
        /// </summary>
        /// <typeparam name="TException">The exception type that causes this precondition to be verified.</typeparam>
        /// <param name="condition">The precondition to verify.</param>
        /// <param name="userMessage">Message to display on contract failure.</param>
        /// <remarks>
        /// All code contract method calls must be at the beginning of the method or property.
        /// The code contract rewriter tool must be used to enable contracts to be verified at runtime.
        /// </remarks>
        public static void Requires<TException> (bool condition, string userMessage) where TException : Exception
        {
            AssertMustUseRewriter (ContractFailureKind.Precondition, "Contract.Requires<TException>");
        }

        /// <summary>
        /// Used within a postcondition to represent the return value.
        /// </summary>
        /// <typeparam name="T">The type of the return value.</typeparam>
        /// <returns>The return value.</returns>
        /// <remarks>
        /// The code contract rewriter tool must be used for this method to be effective.
        /// Otherwise the default value for the type is returned.
        /// </remarks>
        public static T Result<T> ()
        {
            // Marker method, no code required.
            return default (T);
        }

        /// <summary>
        /// Used within a postcondition to represent the final value of an out parameter.
        /// </summary>
        /// <typeparam name="T">The type of the out parameter.</typeparam>
        /// <param name="value">The out parameter.</param>
        /// <returns>The final value of the out parameter.</returns>
        /// <remarks>
        /// The code contract rewriter tool must be used for this method to be effective.
        /// Otherwise the default value for the type is returned in the out parameter and the return value.
        /// </remarks>
        public static T ValueAtReturn<T> (out T value)
        {
            // Marker method, no code required.
            value = default (T);
            return default (T);
        }

    }

}
