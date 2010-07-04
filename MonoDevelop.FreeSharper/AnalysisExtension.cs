// 
// AnalysisEngine.cs
//  
// Author:
//       Nikhil Sarda <diff.operator@gmail.com>
//		 Michael J. Hutchinson <m.j.hutchinson@gmail.com>
// 
// Copyright (c) 2010 Nikhil Sarda, Michael J. Hutchinson
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

using Mono.Addins;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Content;

using System;
using System.Collections.Generic;

namespace MonoDevelop.FreeSharper
{
	public sealed class AnalysisExtension : TextEditorExtension
	{
		private const int AnalysisDelay = 500;
		
		private static object syncRoot = new object();
		internal static AnalysisEngineService engineCtx = null;
		
		private static ExtensionNodeList extensions;
		
		private static Dictionary<String, AnalysisExtension> editorInstances;
		
		bool disposed;
		
		public override void Initialize ()
		{
			base.Initialize ();
			if(engineCtx == null) {
				lock(syncRoot) {
					if(engineCtx == null) {
						engineCtx = new AnalysisEngineService();
						editorInstances = new Dictionary<String, AnalysisExtension>();
						IdeApp.Workbench.ActiveDocumentChanged += NotifyEngine;
						IdeApp.Workbench.ActiveDocumentChanged += NotifyResultsPad;
					}
				}
			}
			
			if(Document != null) {
				//TODO: Get rid of the document param in Rules
				if(extensions == null) 
					extensions = AddinManager.GetExtensionNodes ("/MonoDevelop/FreeSharper/AnalysisRuleExtensions", typeof(RuleExtensionNode));
				foreach(RuleExtensionNode ext in extensions) {
					if(!ext.Supports(Document.FileName.Extension))
						continue;
					//RuleExtension rule = (RuleExtension)ext.CreateInstance();
					//rule.Initialize(Document);
				}
				
				Document.DocumentParsed += StartAnalysis;	
			}
			
			editorInstances.Add(Document.FileName, this);
		}
		
		public override void Dispose ()
		{
			if (Document != null) {
				Document.DocumentParsed -= StartAnalysis;
				
				engineCtx.Flush(Document);
			
				if(editorInstances.Count<=1) {
					engineCtx.Dispose();
					IdeApp.Workbench.ActiveDocumentChanged -= NotifyEngine;
				}
				
				editorInstances.Remove(Document.FileName);
			}
			
			if (disposed)
				return;
			disposed = true;
			base.Dispose ();
		}
		
		void StartAnalysis (object sender, EventArgs args)
		{
			GLib.Timeout.Add (AnalysisDelay, delegate {
					System.Threading.ThreadPool.QueueUserWorkItem (delegate {
					if (!Document.ParsedDocument.Equals(null) && !Document.ParsedDocument.HasErrors) {
						if (Document.Equals(IdeApp.Workbench.ActiveDocument)) {
							engineCtx.EnqueueTaskWithPriority (new AbstractAnalysisTask(Document));
						} else {
							engineCtx.EnqueueTask (new AbstractAnalysisTask(Document));
						}
					}
				});
				return false;
			});	
		}
		
		void NotifyEngine (object sender, EventArgs args)
		{
			engineCtx.SuspendActiveTask(sender, args);
		}
		
		void NotifyResultsPad (object sender, EventArgs args)
		{
			
		}
	}
}