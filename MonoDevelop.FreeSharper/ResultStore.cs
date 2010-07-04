// 
// ResultStore.cs
//  
// Author:
//       Nikhil Sarda <diff.operator@gmail.com>
// 
// Copyright (c) 2010 nikhil
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MonoDevelop.Core;
using MonoDevelop.Projects;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Content;
using MonoDevelop.Ide;

namespace MonoDevelop.FreeSharper
{
	
	public class ResultStore: IEnumerable<RuleResult>, ILocationList
	{
		int taskUpdateCount;
		Dictionary<string, RuleResult> results = new Dictionary<string, RuleResult>();
		Dictionary<FilePath,RuleResult[]> resultIndex = new Dictionary<FilePath, RuleResult[]> ();
		
		public event ResultEventHandler ResultsAdded;
		public event ResultEventHandler ResultsRemoved;
		public event ResultEventHandler ResultsChanged;
		
		public Dictionary<string, RuleResult> Results {
			get {
				return results;
			}
		}
		
		public ResultStore ()
		{
			IdeApp.Workspace.FileRenamedInProject += ProjectFileRenamed;
			IdeApp.Workspace.FileRemovedFromProject += ProjectFileRemoved;
		}
				
		void ProjectFileRenamed (object sender, ProjectFileRenamedEventArgs e)
		{
			throw new NotImplementedException();
		}
		
		void ProjectFileRemoved (object sender, ProjectFileEventArgs e)
		{
			throw new NotImplementedException();
		}
		
		public void MarkStale (RuleResult t)
		{
			if(results.ContainsKey(t.ResultID)) {
				results[t.ResultID].IsStale = true;
				OnResultStaled(t);
			}
		}
		
		public void Add (RuleResult result)
		{
			results.Add(result.ResultID, result);
			OnResultAdded (result);
		}
		
		public void AddRange (IEnumerable<RuleResult> newResults)
		{
			throw new NotImplementedException();
		}
		
		public void RemoveRange (IEnumerable<RuleResult> results)
		{
			throw new NotImplementedException();
		}
		
		public void RemoveItemResults (IWorkspaceObject parent)
		{
			throw new NotImplementedException();
		}
		
		public void RemoveItemResults (IWorkspaceObject parent, bool checkHierarchy)
		{
			throw new NotImplementedException();
		}
		
		public void RemoveFileResults (FilePath file)
		{
			throw new NotImplementedException();
		}
		
		public void Remove (RuleResult result)
		{
			if (results.Remove (result.ResultID))
				OnResultRemoved (result);
		}
		
		public void Clear ()
		{
			throw new NotImplementedException();
		}
		
		public void ClearByOwner (object owner)
		{
			throw new NotImplementedException();
		}
		
		public int Count {
			get { return results.Count; }
		}
		
		public IEnumerator<RuleResult> GetEnumerator ()
		{
			return results.Values.GetEnumerator();
		}
		
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return ((IEnumerable)results).GetEnumerator ();
		}

		public IEnumerable<RuleResult> GetOwnerResults (Document doc)
		{
			return from x in results where x.Value.ResultDoc == doc select x.Value;
		}

		public RuleResult[] GetFileResults (FilePath file)
		{
			throw new NotImplementedException();
		}
		
		public IEnumerable<RuleResult> GetItemResults (IWorkspaceObject parent)
		{
			throw new NotImplementedException();
			return null;
		}
		
		void NotifyResultsAdded (IEnumerable<RuleResult> ts)
		{
			throw new NotImplementedException();
		}
		
		void NotifyResultsChanged (IEnumerable<RuleResult> ts)
		{
			throw new NotImplementedException();
		}
		
		void NotifyResultsRemoved (IEnumerable<RuleResult> ts)
		{
			throw new NotImplementedException();
		}
		
		internal void OnResultAdded (RuleResult t)
		{
			// TODO
		}
		
		internal void OnResultRemoved (RuleResult t)
		{
			//TODO
		}
		
		internal void OnResultStaled (RuleResult t)
		{
			//TODO
		}
		
		int IndexOfResult (RuleResult t)
		{
			return results.ToList().IndexOf(results.First(x => x.Value.Equals(t.ResultID)));
		}
		
		#region ILocationList implementation
		public NavigationPoint GetNextLocation ()
		{
			throw new System.NotImplementedException();
		}
		
		
		public NavigationPoint GetPreviousLocation ()
		{
			throw new System.NotImplementedException();
		}
		
		
		#endregion
		public string ItemName {
			get; set;
		}
	}
	
	public delegate void ResultEventHandler (object sender, ResultEventArgs e);
	
	public class ResultEventArgs : EventArgs
	{
		IEnumerable<RuleResult> results;
	
		public ResultEventArgs (RuleResult result) : this (new RuleResult[] { result })
		{
		}
	
		public ResultEventArgs (IEnumerable<RuleResult> results)
		{
			this.results = results;
		}
	
		public IEnumerable<RuleResult> Results
		{
			get { return results; }
		}
	}
	
}