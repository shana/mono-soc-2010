//
// System.Diagnostics.Contracts.ContractPublicPropertyNameAttribute.cs
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
    /// Specifies a public property name associated with a field, allowing a less-visible field to be used
    /// in contracts within a method.
    /// </summary>
    [Conditional ("CONTRACTS_FULL")]
    [AttributeUsage (AttributeTargets.Field)]
    public sealed class ContractPublicPropertyNameAttribute : Attribute {
        string name;

        public ContractPublicPropertyNameAttribute (string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return name; }
        }
    }
}
#endif