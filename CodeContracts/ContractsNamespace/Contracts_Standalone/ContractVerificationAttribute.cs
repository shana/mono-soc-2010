//
// System.Diagnostics.Contracts.ContractInvariantMethodAttribute.cs
//
// Authors:
// 	Gonzalo Paniagua Javier (gonzalo@ximian.com)
//
// Copyright (c) 2010 Novell, Inc (http://www.novell.com)
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
#if NET_2_1 || NET_4_0
using System;
namespace System.Diagnostics.Contracts {

    /// <summary>
    /// Indicates whether the code contract verification tools should verify this item.
    /// If not, the item is assumed to be correct.
    /// </summary>
    [Conditional ("CONTRACTS_FULL")]
    [AttributeUsageAttribute (AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property)]
    public sealed class ContractVerificationAttribute : Attribute {
        bool val;

        public ContractVerificationAttribute (bool value)
        {
            val = value;
        }

        /// <summary>
        /// Gets whether to include this item in verification.
        /// </summary>
        public bool Value
        {
            get { return val; }
        }
    }
}
#endif

