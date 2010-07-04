// 
// VisitorMetadata.cs
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
using System.Linq;

using MonoDevelop.CSharp.Dom;

namespace MonoDevelop.FreeSharper
{
	public class VisitorMetadata
	{
		public class TypeInfo
		{	
			public String TypeName {
				get; private set;
			}
						
			public String TypeClass {
				get; private set;
			}
			
			public ICSharpNode TypeReference {
				get; private set;
			}
			
			public bool IsProperty {
				get; private set;
			}
			
			public bool IsUsed {
				get; set;
			}
			
			private TypeInfo()
			{
			}
			
			public static TypeInfo TypeInfoFactory (ICSharpNode node)
			{
				if(node.Equals(null))
					throw new ArgumentNullException("Null node passed by visitor");
				TypeInfo typeReturn = new TypeInfo();
				if(node is VariableInitializer) {
					typeReturn.TypeReference = node as VariableInitializer;
					typeReturn.TypeName = ((VariableInitializer)node).Name;
					//TODO How do I get the class?
					typeReturn.TypeClass = ((VariableInitializer)node).NameIdentifier.Name;
					typeReturn.IsUsed = false;
				} else if (node is PropertyDeclaration) {
					typeReturn.TypeReference = node as PropertyDeclaration;
					typeReturn.TypeName = ((PropertyDeclaration)node).Name;
					//TODO How do I get the class?
					typeReturn.IsUsed = false;
					typeReturn.IsProperty = true;
				}
				return typeReturn;
			}
		}
		
		private MethodDeclaration activeMethod;
		public MethodDeclaration ActiveMethod {
			get {
				return activeMethod;
			}
			set {
				ActiveMethodVariables.Clear();
				activeMethod = value;
			}
		}
		
		public Dictionary<String, TypeInfo> ActiveClassVariables {
			get; private set;
		}
		
		public Dictionary<String, TypeInfo> ActiveMethodVariables {
			get; private set;
		}
		
		private VisitorMetadata ()
		{
			ActiveMethodVariables = new Dictionary<string, TypeInfo>();
			ActiveClassVariables = new Dictionary<string, TypeInfo>();
		}
		
		public void AddActiveMethodVariable(VariableInitializer variableInitializer)
		{
			if(!ActiveClassVariables.ContainsKey(variableInitializer.Name)) {
				TypeInfo variableMetadata = TypeInfo.TypeInfoFactory(variableInitializer);
				ActiveClassVariables.Add(variableMetadata.TypeName, variableMetadata);
			}
		}
		
		public void AddActiveClassVariable()
		{
			
		}
		
		public IEnumerable<VisitorMetadata.TypeInfo> UnusedMethodVariables()
		{
			foreach(var keyval in ActiveMethodVariables.Where(keyval => keyval.Value.IsUsed == false)) {
				yield return keyval.Value;
			}
		}
		
		public IEnumerable<VisitorMetadata.TypeInfo> UnusedClassVariables()
		{
			foreach(var keyval in ActiveClassVariables.Where(keyval => keyval.Value.IsUsed == false)) {
				yield return keyval.Value;
			}
		}
		
		public static VisitorMetadata MetaDataFactory() 
		{
			return new VisitorMetadata();
		}
	}
}