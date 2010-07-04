// 
// ResultsPad.cs
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
using Gtk;
using MonoDevelop.Components.Docking;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Ide.Tasks;
using MonoDevelop.Projects;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
namespace MonoDevelop.FreeSharper
{
	public class ResultsPad : IPadContent
	{
		private HPaned control;
		private ScrolledWindow sw;
		private MonoDevelop.Ide.Gui.Components.PadTreeView view;
		private LogView outputView;
		private ListStore resultsStore;
		private TreeModelFilter filter;
		private TreeModelSort sort;
		private ToggleButton errorButton, warningButton, conventionButton, suggestionButton;
		public Dictionary<string, RuleResult> results = new Dictionary<string, RuleResult>();
		int infoCount;
		bool initialLogShow = true;
		
		Menu menu;
		Dictionary<ToggleAction, int> columnsActions = new Dictionary<ToggleAction, int> ();
		Clipboard clipboard;

		int errorCount, warningCount, suggestionCount, conventionCount;
		
		Gdk.Pixbuf iconWarning;
		Gdk.Pixbuf iconError;
		Gdk.Pixbuf iconInfo;
		Gdk.Pixbuf iconSuggestion;
		
		ResultService resultsRepo = ResultService.Instance;
		
		static class DataColumns
		{
			internal const int Type = 0;
			internal const int Read = 1;
			internal const int Result = 2;
		}
		
		static class VisibleColumns
		{
			internal const int Type        = 0;
			internal const int Marked      = 1;
			internal const int Line        = 2;
			internal const int Description = 3;
			internal const int File        = 4;
			internal const int Project     = 5;
			internal const int Path        = 6;
		}

		public Gtk.Widget Control {
			get {
				if (control == null)
					CreateControl ();
				return control;
			}
		}
		
		public IProgressMonitor GetBuildProgressMonitor ()
		{
			if (control == null)
				CreateControl ();
			return outputView.GetProgressMonitor ();
		}
		
		public string Id {
			get { return "MonoDevelop.FreeSharper.ResultsPad"; }
		}

		void IPadContent.Initialize (IPadWindow window)
		{
			window.Title = GettextCatalog.GetString ("Analysis Results");
			//TODO; Need an icon
			window.Icon = MonoDevelop.Ide.Gui.Stock.TipOfTheDay;
			
			DockItemToolbar toolbar = window.GetToolbar (PositionType.Top);
			
			errorButton = new ToggleButton ();
			errorButton.Image = new Gtk.Image (Gtk.Stock.DialogError, Gtk.IconSize.Menu);
			errorButton.Image.Show ();
			errorButton.Toggled += new EventHandler (FilterChanged);
			errorButton.TooltipText = GettextCatalog.GetString ("View errors");
			UpdateErrors();
			toolbar.Add (errorButton);
			
			warningButton = new ToggleButton ();
			warningButton.Image = new Gtk.Image (Gtk.Stock.DialogWarning, Gtk.IconSize.Menu);
			warningButton.Image.Show ();
			warningButton.Toggled += new EventHandler (FilterChanged);
			warningButton.TooltipText = GettextCatalog.GetString ("View warnings");
			UpdateWarningsNum();
			toolbar.Add (warningButton);
			
			conventionButton = new ToggleButton ();
			conventionButton.Image = new Gtk.Image (Gtk.Stock.DialogInfo, Gtk.IconSize.Menu);
			conventionButton.Image.Show ();
			conventionButton.Toggled += new EventHandler (FilterChanged);
			conventionButton.TooltipText = GettextCatalog.GetString ("View conventions");
			UpdateConventionsNum();
			toolbar.Add (conventionButton);
			
			suggestionButton = new ToggleButton ();
			suggestionButton.Image = ImageService.GetImage ("md-message-log", Gtk.IconSize.Menu);
			suggestionButton.Image.Show ();
			suggestionButton.TooltipText = GettextCatalog.GetString ("View suggestions");
			suggestionButton.Toggled += new EventHandler(FilterChanged);
			UpdateSuggestionsNum();
			toolbar.Add (suggestionButton);
			
			toolbar.ShowAll();
		}
		
		void CreateControl ()
		{
			control = new HPaned ();

			resultsStore = new Gtk.ListStore (typeof (Gdk.Pixbuf), // image - type
			                           typeof (bool),       // read?
			                           typeof (RuleResult));       // read? -- use Pango weight

			TreeModelFilterVisibleFunc filterFunct = new TreeModelFilterVisibleFunc (FilterResultTypes);
			filter = new TreeModelFilter (resultsStore, null);
			filter.VisibleFunc = filterFunct;
			
			sort = new TreeModelSort (filter);
			sort.SetSortFunc (VisibleColumns.Type, SeverityIterSort);
			sort.SetSortFunc (VisibleColumns.Project, ProjectIterSort);
			sort.SetSortFunc (VisibleColumns.File, FileIterSort);
			
			view = new MonoDevelop.Ide.Gui.Components.PadTreeView (sort);
			view.RulesHint = true;
			view.PopupMenu += new PopupMenuHandler (OnPopupMenu);
			view.ButtonPressEvent += new ButtonPressEventHandler (OnButtonPressed);
			AddColumns ();
			LoadColumnsVisibility ();
			view.Columns[VisibleColumns.Type].SortColumnId = VisibleColumns.Type;
			view.Columns[VisibleColumns.Project].SortColumnId = VisibleColumns.Project;
			view.Columns[VisibleColumns.File].SortColumnId = VisibleColumns.File;
			
			sw = new Gtk.ScrolledWindow ();
			sw.ShadowType = ShadowType.None;
			sw.Add (view);
			
			IdeApp.Workspace.FirstWorkspaceItemOpened += OnCombineOpen;
			IdeApp.Workspace.LastWorkspaceItemClosed += OnCombineClosed;
			
			view.RowActivated += new RowActivatedHandler (OnRowActivated);
			view.CursorChanged += new EventHandler(OnRowSelected);
			
			iconWarning = sw.RenderIcon (Gtk.Stock.DialogWarning, Gtk.IconSize.Menu, "");
			iconError = sw.RenderIcon (Gtk.Stock.DialogError, Gtk.IconSize.Menu, "");
			iconInfo = sw.RenderIcon (Gtk.Stock.DialogInfo, Gtk.IconSize.Menu, "");
			iconSuggestion = sw.RenderIcon (Gtk.Stock.DialogQuestion, Gtk.IconSize.Menu, "");
			
			control.Add1 (sw);
			
			outputView = new LogView ();
			control.Add2 (outputView);
			
			Control.ShowAll ();
			
			outputView.Hide ();
			
			CreateMenu ();

			// TODO: Load the results pertaining to current document

			control.FocusChain = new Gtk.Widget [] { sw };
		}
		
		bool FilterResultTypes (TreeModel model, TreeIter iter)
		{
			bool canShow = false;

			try {
				RuleResult result = resultsStore.GetValue (iter, DataColumns.Result) as RuleResult;
				if (result == null)
					return true;
				if (result.ResultMessageType == ResultMessageType.Error && errorButton.Active) canShow = true;
				else if (result.ResultMessageType == ResultMessageType.Suggestion && suggestionButton.Active) canShow = true;
				else if (result.ResultMessageType == ResultMessageType.Warning && warningButton.Active) canShow = true;
				else if (result.ResultMessageType == ResultMessageType.Convention && conventionButton.Active) canShow = true;
				else if (result.ResultMessageType == ResultMessageType.Default) canShow = true;
			} catch {
				return false;
			}
			
			return canShow;
		}
		
		void LoadColumnsVisibility ()
		{
			string columns = (string)PropertyService.Get ("Monodevelop.ErrorListColumns", "TRUE;TRUE;TRUE;TRUE;TRUE;TRUE;TRUE");
			string[] tokens = columns.Split (new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);
			if (tokens.Length == 7 && view != null && view.Columns.Length == 7)
			{
				for (int i = 0; i < 7; i++)
				{
					bool visible;
					if (bool.TryParse (tokens[i], out visible))
						view.Columns[i].Visible = visible;
				}
			}
		}

		void StoreColumnsVisibility ()
		{
			string columns = String.Format ("{0};{1};{2};{3};{4};{5};{6}",
			                                view.Columns[VisibleColumns.Type].Visible,
			                                view.Columns[VisibleColumns.Marked].Visible,
			                                view.Columns[VisibleColumns.Line].Visible,
			                                view.Columns[VisibleColumns.Description].Visible,
			                                view.Columns[VisibleColumns.File].Visible,
			                                view.Columns[VisibleColumns.Project].Visible,
			                                view.Columns[VisibleColumns.Path].Visible);
			PropertyService.Set ("Monodevelop.ErrorListColumns", columns);
		}
		
		public void RedrawContent()
		{
		}

		void CreateMenu ()
		{
			if (menu == null)
			{
				ActionGroup group = new ActionGroup ("Popup");

				//Popup menu, to show reference
				Gtk.Action help = new Gtk.Action ("help", GettextCatalog.GetString ("Show result reference"),
				                          GettextCatalog.GetString ("Show result reference"), Gtk.Stock.Help);
				help.Activated += new EventHandler (OnShowReference);
				group.Add (help, "F1");

				//Popup menu, to copy the message
				Gtk.Action copy = new Gtk.Action ("copy", GettextCatalog.GetString ("_Copy"),
				                          GettextCatalog.GetString ("Copy message"), Gtk.Stock.Copy);
				copy.Activated += new EventHandler (OnResultCopied);
				group.Add (copy, "<Control><Mod2>c");

				//Popup menu, to go to location
				Gtk.Action jump = new Gtk.Action ("jump", GettextCatalog.GetString ("_Go to"),
				                          GettextCatalog.GetString ("Go to location"), Gtk.Stock.JumpTo);
				jump.Activated += new EventHandler (OnResultJumpto);
				group.Add (jump);

				Gtk.Action columns = new Gtk.Action ("columns", GettextCatalog.GetString ("Columns"));
				group.Add (columns, null);

				ToggleAction columnType = new ToggleAction ("columnType", GettextCatalog.GetString ("Type"),
				                                            GettextCatalog.GetString ("Toggle visibility of Type column"), null);
				columnType.Toggled += new EventHandler (OnColumnVisibilityChanged);
				columnsActions[columnType] = VisibleColumns.Type;
				group.Add (columnType);

				ToggleAction columnValidity = new ToggleAction ("columnValidity", GettextCatalog.GetString ("Validity"),
				                                                GettextCatalog.GetString ("Toggle visibility of Validity column"), null);
				columnValidity.Toggled += new EventHandler (OnColumnVisibilityChanged);
				columnsActions[columnValidity] = VisibleColumns.Marked;
				group.Add (columnValidity);

				ToggleAction columnLine = new ToggleAction ("columnLine", GettextCatalog.GetString ("Line"),
				                                            GettextCatalog.GetString ("Toggle visibility of Line column"), null);
				columnLine.Toggled += new EventHandler (OnColumnVisibilityChanged);
				columnsActions[columnLine] = VisibleColumns.Line;
				group.Add (columnLine);

				ToggleAction columnDescription = new ToggleAction ("columnDescription", GettextCatalog.GetString ("Description"),
				                                                   GettextCatalog.GetString ("Toggle visibility of Description column"), null);
				columnDescription.Toggled += new EventHandler (OnColumnVisibilityChanged);
				columnsActions[columnDescription] = VisibleColumns.Description;
				group.Add (columnDescription);

				ToggleAction columnFile = new ToggleAction ("columnFile", GettextCatalog.GetString ("File"),
				                                            GettextCatalog.GetString ("Toggle visibility of File column"), null);
				columnFile.Toggled += new EventHandler (OnColumnVisibilityChanged);
				columnsActions[columnFile] = VisibleColumns.File;
				group.Add (columnFile);

				ToggleAction columnProject = new ToggleAction ("columnProject", GettextCatalog.GetString ("Project"),
				                                            GettextCatalog.GetString ("Toggle visibility of Project column"), null);
				columnProject.Toggled += new EventHandler (OnColumnVisibilityChanged);
				columnsActions[columnProject] = VisibleColumns.Project;
				group.Add (columnProject);

				ToggleAction columnPath = new ToggleAction ("columnPath", GettextCatalog.GetString ("Path"),
				                                            GettextCatalog.GetString ("Toggle visibility of Path column"), null);
				columnPath.Toggled += new EventHandler (OnColumnVisibilityChanged);
				columnsActions[columnPath] = VisibleColumns.Path;
				group.Add (columnPath);

				UIManager uiManager = new UIManager ();
				uiManager.InsertActionGroup (group, 0);
				
				string uiStr = "<ui><popup name='popup'>"
					+ "<menuitem action='help'/>"
					+ "<menuitem action='copy'/>"
					+ "<menuitem action='jump'/>"
					+ "<separator/>"
					+ "<menu action='columns'>"
					+ "<menuitem action='columnType' />"
					+ "<menuitem action='columnValidity' />"
					+ "<menuitem action='columnLine' />"
					+ "<menuitem action='columnDescription' />"
					+ "<menuitem action='columnFile' />"
					+ "<menuitem action='columnProject' />"
					+ "<menuitem action='columnPath' />"
					+ "</menu>"
					+ "</popup></ui>";

				uiManager.AddUiFromString (uiStr);
				menu = (Menu)uiManager.GetWidget ("/popup");
				menu.ShowAll ();

				menu.Shown += delegate (object o, EventArgs args)
				{
					columnType.Active = view.Columns[VisibleColumns.Type].Visible;
					columnValidity.Active = view.Columns[VisibleColumns.Marked].Visible;
					columnLine.Active = view.Columns[VisibleColumns.Line].Visible;
					columnDescription.Active = view.Columns[VisibleColumns.Description].Visible;
					columnFile.Active = view.Columns[VisibleColumns.File].Visible;
					columnProject.Active = view.Columns[VisibleColumns.Project].Visible;
					columnPath.Active = view.Columns[VisibleColumns.Path].Visible;
					help.Sensitive = copy.Sensitive = jump.Sensitive =
						view.Selection != null &&
						view.Selection.CountSelectedRows () > 0 &&
						(columnType.Active ||
						columnValidity.Active ||
						columnLine.Active ||
						columnDescription.Active ||
						columnFile.Active ||
						columnPath.Active);
				};
			}
		}


		[GLib.ConnectBefore]
		void OnButtonPressed (object o, ButtonPressEventArgs args)
		{
			if (args.Event.Button == 3)
				menu.Popup ();
		}

		void OnPopupMenu (object o, PopupMenuArgs args)
		{
			menu.Popup ();
		}

		RuleResult SelectedResult
		{
			get {
				TreeModel model;
				TreeIter iter;
				if (view.Selection.GetSelected (out model, out iter)) 
					return model.GetValue (iter, DataColumns.Result) as RuleResult;
				return null; // no one selected
			}
		}

		void OnResultCopied (object o, EventArgs args)
		{
			RuleResult result = SelectedResult;
			if (result != null) {
				StringBuilder text = new StringBuilder ();
				if (!string.IsNullOrEmpty (result.ResultDoc.FileName)) {
					text.Append (result.ResultDoc.FileName);
					text.Append (": ");
				}
				switch(result.ResultMessageType) {
				case ResultMessageType.Convention:
					text.Append ("Convention");
					break;
				case ResultMessageType.Error:
					text.Append ("Error");
					break;
				case ResultMessageType.Suggestion:
					text.Append("Suggestion");
					break;
				case ResultMessageType.Warning:
					text.Append("Warning");
					break;
				default:
					break;
				}
				text.Append (": ");
				text.Append (result.Message);
								
				if (result.ResultDoc.Project != null)
					text.Append (" (").Append (result.ResultDoc.Project.Name).Append (")");
				
				clipboard = Clipboard.Get (Gdk.Atom.Intern ("CLIPBOARD", false));
				clipboard.Text = text.ToString ();
				clipboard = Clipboard.Get (Gdk.Atom.Intern ("PRIMARY", false));
				clipboard.Text = text.ToString ();
			}
		}

		void OnShowReference (object o, EventArgs args)
		{
			string reference = null;
			if (GetSelectedResultReference (out reference)) {
				IdeApp.HelpOperations.ShowHelp ("error:" + reference);
				return;
			}
		}

		bool GetSelectedResultReference (out string reference)
		{
			RuleResult result = SelectedResult;
			if (result != null && !String.IsNullOrEmpty (result.ResultDoc.Name)) {
				reference = result.ResultMessageType.ToString();
				return true;
			}
			reference = null;
			return false;
		}

		void OnResultJumpto (object o, EventArgs args)
		{
			TreeIter iter;
			TreeModel model;
			if (view.Selection.GetSelected (out model, out iter)) {
				iter = filter.ConvertIterToChildIter (sort.ConvertIterToChildIter (iter));
				resultsStore.SetValue (iter, DataColumns.Read, true);
				RuleResult result = resultsStore.GetValue (iter, DataColumns.Result) as RuleResult;
				if (result != null) {
					ResultService.ShowStatus (result);
					result.JumpToPosition ();
					//TaskService.Errors.CurrentLocationTask = task;
					//IdeApp.Workbench.ActiveLocationList = TaskService.Errors;
				}
			}
		}

		void OnColumnVisibilityChanged (object o, EventArgs args)
		{
			ToggleAction action = o as ToggleAction;
			if (action != null)
			{
				view.Columns[columnsActions[action]].Visible = action.Active;
				StoreColumnsVisibility ();
			}
		}

		void AddColumns ()
		{
			Gtk.CellRendererPixbuf iconRender = new Gtk.CellRendererPixbuf ();
			
			Gtk.CellRendererToggle toggleRender = new Gtk.CellRendererToggle ();
			toggleRender.Toggled += new ToggledHandler (ItemToggled);
			
			TreeViewColumn col;
			col = view.AppendColumn ("!", iconRender, "pixbuf", DataColumns.Type);
			
			col = view.AppendColumn ("", toggleRender);
			col.SetCellDataFunc (toggleRender, new Gtk.TreeCellDataFunc (ToggleDataFunc));
			
			col = view.AppendColumn (GettextCatalog.GetString ("Line"), view.TextRenderer);
			col.SetCellDataFunc (view.TextRenderer, new Gtk.TreeCellDataFunc (LineDataFunc));
			
			col = view.AppendColumn (GettextCatalog.GetString ("Description"), view.TextRenderer);
			col.SetCellDataFunc (view.TextRenderer, new Gtk.TreeCellDataFunc (DescriptionDataFunc));
			col.Resizable = true;
			
			col = view.AppendColumn (GettextCatalog.GetString ("File"), view.TextRenderer);
			col.SetCellDataFunc (view.TextRenderer, new Gtk.TreeCellDataFunc (FileDataFunc));
			col.Resizable = true;
			
			col = view.AppendColumn (GettextCatalog.GetString ("Project"), view.TextRenderer);
			col.SetCellDataFunc (view.TextRenderer, new Gtk.TreeCellDataFunc (ProjectDataFunc));
			col.Resizable = true;
			
			col = view.AppendColumn (GettextCatalog.GetString ("Path"), view.TextRenderer);
			col.SetCellDataFunc (view.TextRenderer, new Gtk.TreeCellDataFunc (PathDataFunc));
			col.Resizable = true;
		}
		
		static void ToggleDataFunc (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Gtk.CellRendererToggle toggleRenderer = (Gtk.CellRendererToggle)cell;
			RuleResult result = model.GetValue (iter, DataColumns.Result) as RuleResult; 
			if (result == null)
				return;
			toggleRenderer.Active = result.IsStale;
		}
		
		static void LineDataFunc (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Gtk.CellRendererText textRenderer = (Gtk.CellRendererText)cell;
			RuleResult result = model.GetValue (iter, DataColumns.Result) as RuleResult; 
			if (result == null)
				return;
			SetText (textRenderer, model, iter, result, result.Line != 0 ? result.Line.ToString () : "");
		}
		
		static void DescriptionDataFunc (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Gtk.CellRendererText textRenderer = (Gtk.CellRendererText)cell;
			RuleResult result = model.GetValue (iter, DataColumns.Result) as RuleResult; 
			if (result == null)
				return;
			SetText (textRenderer, model, iter, result, result.Message);
		}
		
		static void FileDataFunc (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Gtk.CellRendererText textRenderer = (Gtk.CellRendererText)cell;
			RuleResult result = model.GetValue (iter, DataColumns.Result) as RuleResult; 
			if (result == null)
				return;
			
			string tmpPath = result.GetPath();
			string fileName;
			try {
				fileName = Path.GetFileName (tmpPath);
			} catch (Exception) { 
				fileName =  tmpPath;
			}
			
			SetText (textRenderer, model, iter, result, fileName);
		}
		
		static void ProjectDataFunc (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Gtk.CellRendererText textRenderer = (Gtk.CellRendererText)cell;
			RuleResult result = model.GetValue (iter, DataColumns.Result) as RuleResult; 
			if (result == null)
				return;
			SetText (textRenderer, model, iter, result, result.GetProject());
		}
		
		
		static void PathDataFunc (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			Gtk.CellRendererText textRenderer = (Gtk.CellRendererText)cell;
			RuleResult result = model.GetValue (iter, DataColumns.Result) as RuleResult; 
			if (result == null)
				return;
			SetText (textRenderer, model, iter, result, result.GetPath());
		}
		
		static void SetText (CellRendererText textRenderer, TreeModel model, TreeIter iter, RuleResult result, string text)
		{
			textRenderer.Text = text;
			textRenderer.Weight = (int)((bool)model.GetValue (iter, DataColumns.Read) ? Pango.Weight.Normal : Pango.Weight.Bold);
		}
		
		void OnCombineOpen(object sender, EventArgs e)
		{
			Clear();
		}
		
		void OnCombineClosed(object sender, EventArgs e)
		{
			Clear();
		}
		
		public void Dispose ()
		{
		}
		
		void OnRowActivated (object o, RowActivatedArgs args)
		{
			OnResultJumpto (null, null);
		}
		
		void OnRowSelected (object o, EventArgs args)
		{
			TreeIter iter;
			TreeModel model;
			if (view.Selection.GetSelected (out model, out iter)) {
				iter = filter.ConvertIterToChildIter (sort.ConvertIterToChildIter (iter));
				resultsStore.SetValue (iter, DataColumns.Read, true);
				RuleResult result = resultsStore.GetValue (iter, DataColumns.Result) as RuleResult;
			if (result != null) {
				ResultService.ShowStatus (result);
				}
			}
		}
		
		public CompilerResults CompilerResults = null;
		
		void FilterChanged (object sender, EventArgs e)
		{
			//if(results.Count > 0)
			//	MessageService.ShowMessage("yo");
			filter.Refilter();
		}

		public void ShowResults (object sender, EventArgs e)
		{
			Clear();

		}

		private void Clear()
		{
			errorCount = warningCount = suggestionCount = conventionCount = 0;
			resultsStore.Clear();
			resultsRepo.Flush();
			UpdateErrors();
			UpdateWarningsNum();
			UpdateConventionsNum();
			UpdateSuggestionsNum();
		}
		
		public void AddResults (IEnumerable<RuleResult> results)
		{
			int n = 1;
			foreach (var t in results) {
				AddResultInternal (t);
				if ((n++ % 100) == 0) {
					DispatchService.RunPendingEvents ();
				}
			}
			filter.Refilter ();
		}
		
		public void AddResult (RuleResult t)
		{
			AddResultInternal (t);
			filter.Refilter ();
		}
		
		void AddResultInternal (RuleResult t)
		{
			if (results.ContainsKey (t.ResultID)) return;
			
			Gdk.Pixbuf stock = null;
			
			switch (t.ResultMessageType) {
				case ResultMessageType.Error:
					stock = iconError;
					errorCount++;
					UpdateErrors ();
					break; 
				case ResultMessageType.Warning:
					stock = iconWarning;
					warningCount++;
					UpdateWarningsNum ();	
					break;
				case ResultMessageType.Suggestion:
					stock = iconSuggestion;
					suggestionCount++;
					UpdateSuggestionsNum ();
					break;
				case ResultMessageType.Convention:
					stock = iconInfo;
					conventionCount++;
					UpdateConventionsNum ();
					break;
				default:
					break;
			}
			
			results.Add(t.ResultID, t);
			
			resultsStore.AppendValues (stock, false, t);
		}

		#region Incontrol code
		void UpdateErrors () 
		{
			errorButton.Label = " " + string.Format(GettextCatalog.GetPluralString("{0} Error", "{0} Errors", errorCount), errorCount);
			errorButton.Image.Show ();
		}

		void UpdateWarningsNum ()
		{
			warningButton.Label = " " + string.Format(GettextCatalog.GetPluralString("{0} Warning", "{0} Warnings", warningCount), warningCount); 
			warningButton.Image.Show ();
		}

		void UpdateConventionsNum ()
		{
			conventionButton.Label = " " + string.Format(GettextCatalog.GetPluralString("{0} Convention", "{0} Conventions", infoCount), infoCount);
			conventionButton.Image.Show ();
		}
		
		void UpdateSuggestionsNum()
		{
			suggestionButton.Label = " " + string.Format(GettextCatalog.GetPluralString("{0} Suggestion", "{0} Suggestions", suggestionCount), suggestionCount);
			conventionButton.Image.Show ();
		}
		#endregion Incontrol code
		
		public event EventHandler<TaskEventArgs> TaskToggled;
		protected virtual void OnTaskToggled (TaskEventArgs e)
		{
			EventHandler<TaskEventArgs> handler = this.TaskToggled;
			if (handler != null)
				handler (this, e);
		}
		
		private void ItemToggled (object o, ToggledArgs args)
		{
			
		}

		static int SeverityIterSort(TreeModel model, TreeIter a, TreeIter z)
		{
			RuleResult aResult = model.GetValue(a, DataColumns.Result) as RuleResult ,
			     zResult = model.GetValue(z, DataColumns.Result) as RuleResult;
			     
			return (aResult != null && zResult != null) ?
			       aResult.ResultMessageType.ToString().CompareTo(aResult.ResultMessageType.ToString()) :
			       0;
		}
		
		static int ProjectIterSort (TreeModel model, TreeIter a, TreeIter z)
		{
			RuleResult aRuleResult = model.GetValue (a, DataColumns.Result) as RuleResult,
			     zRuleResult = model.GetValue (z, DataColumns.Result) as RuleResult;
			     
			return (aRuleResult != null && zRuleResult != null) ?
			       aRuleResult.GetPath().CompareTo (zRuleResult.GetPath()) :
			       0;
		}
		
		static int FileIterSort (TreeModel model, TreeIter a, TreeIter z)
		{
			RuleResult aRuleResult = model.GetValue (a, DataColumns.Result) as RuleResult,
			     zRuleResult = model.GetValue (z, DataColumns.Result) as RuleResult;
			     
			return (aRuleResult != null && zRuleResult != null) ?
			       aRuleResult.ResultDoc.FileName.CompareTo (zRuleResult.ResultDoc.FileName) :
			       0;
		}		
	}
}

