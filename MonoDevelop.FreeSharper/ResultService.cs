// 
// ResultRepository.cs
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
using System.Threading;
using Mono.TextEditor;
using MonoDevelop.Ide;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Core.Serialization;
using MonoDevelop.Projects;
using System.IO;

namespace MonoDevelop.FreeSharper
{
	public class ResultService 
	{
		public class SweepWorkerThread : WorkerThread
		{
			private static object syncRoot = new object();
			
			private ResultService currentRepository;
			
			private static bool busy;
			public static bool IsBusy {
				get {
					return busy;
				}
			}
		
			public SweepWorkerThread (ResultService r)
			{
				this.currentRepository = r;
				busy = true;
			}
			
			protected override void InnerRun ()
			{
				//TODO: Remove from result store
				busy = false;
				base.Stop ();
			}
		}
		
		private static ResultService instance = new ResultService();
		public static ResultService Instance {
			get {
				return instance;
			}
		}
		
		static ResultStore resultStore = new ResultStore();
		
		SweepWorkerThread sweepWorker;
		
		public void MarkStale (int Line) 
		{
			foreach(var r in resultStore.Where(x => x.Line == Line))
				resultStore.MarkStale(r);
		}
		
		private ResultService()
		{
			Timer sweepTimer = new Timer(delegate { 
				if(!SweepWorkerThread.IsBusy) {
					sweepWorker = new SweepWorkerThread(this);
					sweepWorker.Start();
				}
			}, null, 0, 1000);
			
			IdeApp.Workspace.WorkspaceItemLoaded += OnWorkspaceItemLoaded;
			IdeApp.Workspace.WorkspaceItemUnloaded += OnWorkspaceItemUnloaded;
		}
		
		public void Flush()
		{
		}
		
		public static void ShowStatus (RuleResult t)
		{
			if (t == null)
				IdeApp.Workbench.StatusBar.ShowMessage (GettextCatalog.GetString ("No more errors or warnings"));
			switch(t.ResultMessageType) {
			case ResultMessageType.Error:
				IdeApp.Workbench.StatusBar.ShowError (t.Message);
				break;
			case ResultMessageType.Warning:
				IdeApp.Workbench.StatusBar.ShowWarning (t.Message);
				break;
			default:
				IdeApp.Workbench.StatusBar.ShowMessage (t.Message);
				break;
			}	
		}
		
		public static void ShowResults ()
		{
			Pad resultsPad = IdeApp.Workbench.GetPad<MonoDevelop.FreeSharper.ResultsPad> ();
			if (resultsPad != null) {
				resultsPad.Visible = true;
				resultsPad.BringToFront ();
			}
		}
		
		static void OnWorkspaceItemLoaded (object sender, WorkspaceItemEventArgs e)
		{
			string fileToLoad = GetUserResultsFilename (e.Item);	
			try {
				if (File.Exists (fileToLoad)) {
					XmlDataSerializer serializer = new XmlDataSerializer (new DataContext ());
					List<RuleResult> rs = (List<RuleResult>) serializer.Deserialize (fileToLoad, typeof(List<RuleResult>));	
					foreach (var t in rs) {
						resultStore.Add (t);
						//ResultDoc is null because it is not serialized, fix this!
					}
				}
			}
			catch (Exception ex) {
				LoggingService.LogWarning ("Could not load analysis results: " + fileToLoad, ex);
			}
		}
		
		static void OnWorkspaceItemUnloaded (object sender, WorkspaceItemEventArgs e)
		{
			SaveAnalysisResults (e.Item);		
			//TODO: remove the corresponding results from the result store
		}
		
		static FilePath GetUserResultsFilename (WorkspaceItem item)
		{
			FilePath combinePath = item.FileName.ParentDirectory;
			return combinePath.Combine (item.FileName.FileNameWithoutExtension + ".userresults");
		}
		
		internal static void SaveAnalysisResults (IWorkspaceObject item)
		{
			/*string fileToSave = GetUserResultsFilename ((WorkspaceItem)item);
			try {
				List<RuleResult> uresults = new List<RuleResult> (resultStore.GetRuleResults (item, true));
				if (uresults.Count == 0) {
					if (File.Exists (fileToSave))
						File.Delete (fileToSave);
				} else {
					XmlDataSerializer serializer = new XmlDataSerializer (new DataContext ());
					serializer.Serialize (fileToSave, uresults);
				}
			} catch (Exception ex) {
				LoggingService.LogWarning ("Could not save analysis results: " + fileToSave, ex);
			}*/
		}
	}
}