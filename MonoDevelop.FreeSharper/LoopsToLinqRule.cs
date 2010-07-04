// 
// LoopsToLinqRule.cs
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
using System.Linq;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.FreeSharper
{
	public class LoopsToLinqRule<T> : AbstractCSharpRule<T>
	{
		//Static data
		private string LinqExpression;
		
		public override void Initialize(Document d)
		{
			RuleId = "Loops to Linq";
			base.Initialize(d);
		}
		
		public override void RunRule (object obj)
		{
			base.RunRule(obj);
			
			if(RuleNode == null)
				return;
			
			/*if((RuleNode as DomType).ClassType == ClassType.Interface) {
				if(!(RuleNode as DomType).Name.StartsWith("I"))	{
					PostResult(new RuleResult(ResultMessageType.Warning, (RuleNode as DomType).Location.Line, NamingConventionRule.MsgInterfaceConvention));   
				}
			}*/
		
			//TODO check for camelCase and PascalCase
		}
	}
}