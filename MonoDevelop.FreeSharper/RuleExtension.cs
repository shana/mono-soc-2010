// 
// RuleExtension.cs
//  
// Author:
//       Nikhil Sarda <diff.operator@gmail.com}>
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

using Mono.Addins;

using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.SourceEditor;
using Mono.TextEditor;

namespace MonoDevelop.FreeSharper
{
	public class RuleExtension : IDisposable
	{
		public string RuleId {
			get; protected set;
		}
		
		public MonoDevelop.Ide.Gui.Document RuleRunDocument {
			get; protected set;
		}

		public string ValidFileExtension {
			get; protected set;
		}
		
		public string ValidParserUnit {
			get; protected set;
		}
		
		public static Priorities RulePriority {
			get; protected set;
		}
		
		public static bool IsCron {
			get; protected set;
		}
		
		public void Dispose ()
		{
		}

		public virtual void RunRule (object obj) 
		{
		}
		
		public virtual void Initialize(MonoDevelop.Ide.Gui.Document d)
		{
			RuleRunDocument = d;
			// Set priorities to medium by default.
			RulePriority = Priorities.Medium;
			AnalysisEngineService.RegisterRule(this);
		}
		
		public virtual bool IsValidForDocument {
			get {
			if(RuleRunDocument.FileName.Extension.Contains(ValidFileExtension))
				return true;
			return false;
			}
		}
		
		public void PostResult(RuleResult result)
		{
			DispatchService.GuiSyncDispatch( delegate {
				//TODO Logic here to post on text editor
				SourceEditorView view = RuleRunDocument.GetContent<SourceEditorView> ();
				TextMarker resultMarker;
				//TODO Figure out some cool animations as these markers are posted
				switch(result.ResultMessageType) {
				case ResultMessageType.Convention:
					break;
				case ResultMessageType.Error:
					//resultMarker = new ErrorTextMarker(new MonoDevelop.Ide.Tasks.Task(), view.Document.Lines[result.LineToMark], true, result.Message);
					break;
				case ResultMessageType.Suggestion:
					break;
				case ResultMessageType.Warning:
					break;
				}
				//view.Document.AddMarker(result.LineToMark, resultMarker);
			});
		}
	}
}

