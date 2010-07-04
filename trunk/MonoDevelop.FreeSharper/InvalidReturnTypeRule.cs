// 
// NamingConventionRule.cs
//  
// Author:
//       Nikhil Sarda <diff.operator@gmail.com>
// 
// Copyright (c) 2010 Nikhil Sarda
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections.Generic;

using MonoDevelop.Ide.Gui;
using MonoDevelop.Projects.Dom;
using MonoDevelop.CSharp.Dom;

namespace MonoDevelop.FreeSharper
{
	/// <summary>
	/// All C# rules must inherit from AbstractCSharpRule
	/// All rules must specify
	/// </summary>
	public class InvalidReturnTypeRule<T> : AbstractCSharpRule<T> where T : MethodDeclaration 
	{
		private const string MsgReturnInvalid = "The return type does not match with the declared return type";
		
		public override void Initialize(Document d)
		{
			//Upon initialization do NOT pass document 
			RuleId = "Invalid Return Type Rule";
			base.Initialize(d);
		}
		
		public void RunRule (T node)
		{
			base.RunRule(node);
			ICSharpNode returnType = node.ReturnType;
			foreach(var child in node.Children)
				RecursiveNodeTraverser(child as ICSharpNode, returnType);
		}
		
		private void RecursiveNodeTraverser(ICSharpNode node, ICSharpNode returnType) 
		{
			if(node is ReturnStatement) {
				//TODO: Check if types are equal
			}
			foreach(var child in (node as AbstractCSharpNode).Children)
				RecursiveNodeTraverser(child as ICSharpNode, returnType);
		}
	}
}

