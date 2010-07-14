using System;
using MonoDevelop.Projects;
using Gtk;
using MonoDevelop.Ide.Gui.Dialogs;
using MonoDevelop.Ide;

namespace MonoDevelop.FSharp.Project
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class CompilationOrderPanelWidget : Gtk.Bin
	{
		DotNetProject project;
		ListStore files = new ListStore(typeof(ProjectFile));
		
		public CompilationOrderPanelWidget (DotNetProject project)
		{
			this.Build ();

			TreeViewColumn column = new TreeViewColumn{ Title = "Virtual file path" };
			CellRendererText renderer = new CellRendererText();
			column.PackStart(renderer, true);
			column.SetCellDataFunc(renderer, new TreeCellDataFunc((col, cell, model, iter) =>
				(cell as CellRendererText).Text = ((ProjectFile)model.GetValue(iter, 0)).ProjectVirtualPath));
			listProjectFiles.AppendColumn(column);

			this.project = project;
			
			foreach(ProjectFile item in project.Files) {
				if (item.BuildAction != "Compile" || item.FilePath.Extension != ".fs") continue;
				files.AppendValues(item);
			}

			listProjectFiles.Model = files;
		}

		protected override void OnDestroyed() {
			if (files != null) {
				files.Dispose();
				files = null;
			}

			base.OnDestroyed();
		}

		public bool ValidateChanges() {
			return true;
		}

		public void Store(MultiConfigItemOptionsDialog dlg) {
			foreach (object[] item in files) {
				var it = item[0] as ProjectFile;
				this.project.Files.Remove(it);
			}

			IdeApp.ProjectOperations.Save(this.project);

			foreach (object[] item in files) {
				var it = item[0] as ProjectFile;
				this.project.Files.Add(it);
			}

			IdeApp.ProjectOperations.Save(this.project);
		}
	}

	public class CompilationOrderPanel : ItemOptionsPanel
	{
		CompilationOrderPanelWidget widget;

		public override Widget CreatePanelWidget() {
			return (widget = new CompilationOrderPanelWidget((DotNetProject)ConfiguredProject));
		}

		public override bool ValidateChanges() {
			return widget.ValidateChanges();
		}

		public override void ApplyChanges() {
			MultiConfigItemOptionsDialog dlg = (MultiConfigItemOptionsDialog)ParentDialog;
			widget.Store(dlg);
		}
	}
}

