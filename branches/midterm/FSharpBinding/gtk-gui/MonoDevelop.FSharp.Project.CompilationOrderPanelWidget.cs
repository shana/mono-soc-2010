
// This file has been generated by the GUI designer. Do not modify.
namespace MonoDevelop.FSharp.Project
{
	public partial class CompilationOrderPanelWidget
	{
		private global::Gtk.VBox vbox1;

		private global::Gtk.ScrolledWindow GtkScrolledWindow;

		private global::Gtk.TreeView listProjectFiles;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget MonoDevelop.FSharp.Project.CompilationOrderPanelWidget
			global::Stetic.BinContainer.Attach (this);
			this.Name = "MonoDevelop.FSharp.Project.CompilationOrderPanelWidget";
			// Container child MonoDevelop.FSharp.Project.CompilationOrderPanelWidget.Gtk.Container+ContainerChild
			this.vbox1 = new global::Gtk.VBox ();
			this.vbox1.Name = "vbox1";
			this.vbox1.Spacing = 12;
			this.vbox1.BorderWidth = ((uint)(6));
			// Container child vbox1.Gtk.Box+BoxChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.listProjectFiles = new global::Gtk.TreeView ();
			this.listProjectFiles.CanFocus = true;
			this.listProjectFiles.Name = "listProjectFiles";
			this.listProjectFiles.HeadersVisible = false;
			this.listProjectFiles.Reorderable = true;
			this.GtkScrolledWindow.Add (this.listProjectFiles);
			this.vbox1.Add (this.GtkScrolledWindow);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.GtkScrolledWindow]));
			w2.Position = 0;
			this.Add (this.vbox1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Show ();
		}
	}
}
