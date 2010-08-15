// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.1433
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------


// This file has been generated by the GUI designer. Do not modify.
namespace MonoDevelop.GtkCore.Dialogs
{
	public partial class ProjectConversionDialog
	{
		private global::Gtk.HBox hbox;

		private global::Gtk.Alignment alignmentLogo;

		private global::Gtk.Image imageLogo;

		private global::Gtk.VBox vbox2;

		private global::Gtk.Label labelProject;

		private global::Gtk.Label labelInfo;

		private global::Gtk.HSeparator hseparator1;

		private global::Gtk.HBox hbox1;

		private global::Gtk.Label labelFolder;

		private global::Gtk.Entry entryFolder;

		private global::Gtk.CheckButton checkBackup;

		private global::Gtk.Button buttonConvert;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget MonoDevelop.GtkCore.Dialogs.ProjectConversionDialog
			this.Name = "MonoDevelop.GtkCore.Dialogs.ProjectConversionDialog";
			this.Title = global::Mono.Unix.Catalog.GetString ("Project Name");
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			this.Modal = true;
			this.SkipTaskbarHint = true;
			// Internal child MonoDevelop.GtkCore.Dialogs.ProjectConversionDialog.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.hbox = new global::Gtk.HBox ();
			this.hbox.Name = "hbox";
			this.hbox.Spacing = 6;
			// Container child hbox.Gtk.Box+BoxChild
			this.alignmentLogo = new global::Gtk.Alignment (1f, 0.15f, 1f, 0f);
			this.alignmentLogo.Name = "alignmentLogo";
			// Container child alignmentLogo.Gtk.Container+ContainerChild
			this.imageLogo = new global::Gtk.Image ();
			this.imageLogo.WidthRequest = 180;
			this.imageLogo.Name = "imageLogo";
			this.imageLogo.Yalign = 0f;
			this.imageLogo.Pixbuf = global::Gdk.Pixbuf.LoadFromResource ("gtk-logo.png");
			this.alignmentLogo.Add (this.imageLogo);
			this.hbox.Add (this.alignmentLogo);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox[this.alignmentLogo]));
			w3.Position = 0;
			w3.Expand = false;
			w3.Fill = false;
			// Container child hbox.Gtk.Box+BoxChild
			this.vbox2 = new global::Gtk.VBox ();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			// Container child vbox2.Gtk.Box+BoxChild
			this.labelProject = new global::Gtk.Label ();
			this.labelProject.Name = "labelProject";
			this.labelProject.LabelProp = global::Mono.Unix.Catalog.GetString ("<b><big>GTK# Project Conversion</big></b>");
			this.labelProject.UseMarkup = true;
			this.vbox2.Add (this.labelProject);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.labelProject]));
			w4.Position = 0;
			w4.Expand = false;
			w4.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.labelInfo = new global::Gtk.Label ();
			this.labelInfo.Name = "labelInfo";
			this.labelInfo.Xalign = 0f;
			this.labelInfo.Yalign = 0f;
			this.labelInfo.LabelProp = global::Mono.Unix.Catalog.GetString ("This project has been created in the previous\nversion of GTK# addin and must be converted. \n\n<b>Following changes will be made :</b>\n\t- split gui.stetic into separate .gtkx files\n\t- split generated.cs into separate helper classes\n\t- remove gtk-gui folder.\n\t- create a designer folder for stock icons\n\t  and generated helper classes. ");
			this.labelInfo.UseMarkup = true;
			this.labelInfo.Wrap = true;
			this.vbox2.Add (this.labelInfo);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.labelInfo]));
			w5.Position = 1;
			w5.Expand = false;
			w5.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.hseparator1 = new global::Gtk.HSeparator ();
			this.hseparator1.Name = "hseparator1";
			this.vbox2.Add (this.hseparator1);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.hseparator1]));
			w6.Position = 2;
			w6.Expand = false;
			w6.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox ();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.labelFolder = new global::Gtk.Label ();
			this.labelFolder.Name = "labelFolder";
			this.labelFolder.LabelProp = global::Mono.Unix.Catalog.GetString ("Designer folder name:");
			this.hbox1.Add (this.labelFolder);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.labelFolder]));
			w7.Position = 0;
			w7.Expand = false;
			w7.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.entryFolder = new global::Gtk.Entry ();
			this.entryFolder.CanFocus = true;
			this.entryFolder.Name = "entryFolder";
			this.entryFolder.IsEditable = true;
			this.entryFolder.InvisibleChar = '●';
			this.hbox1.Add (this.entryFolder);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.entryFolder]));
			w8.Position = 1;
			this.vbox2.Add (this.hbox1);
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.hbox1]));
			w9.Position = 3;
			w9.Expand = false;
			w9.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.checkBackup = new global::Gtk.CheckButton ();
			this.checkBackup.CanFocus = true;
			this.checkBackup.Name = "checkBackup";
			this.checkBackup.Label = global::Mono.Unix.Catalog.GetString ("Make a backup before converting");
			this.checkBackup.Active = true;
			this.checkBackup.DrawIndicator = true;
			this.checkBackup.UseUnderline = true;
			this.vbox2.Add (this.checkBackup);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.checkBackup]));
			w10.Position = 4;
			w10.Expand = false;
			w10.Fill = false;
			this.hbox.Add (this.vbox2);
			global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.hbox[this.vbox2]));
			w11.Position = 1;
			w11.Expand = false;
			w11.Fill = false;
			w1.Add (this.hbox);
			global::Gtk.Box.BoxChild w12 = ((global::Gtk.Box.BoxChild)(w1[this.hbox]));
			w12.Position = 0;
			w12.Expand = false;
			w12.Fill = false;
			// Internal child MonoDevelop.GtkCore.Dialogs.ProjectConversionDialog.ActionArea
			global::Gtk.HButtonBox w13 = this.ActionArea;
			w13.Name = "dialog1_ActionArea";
			w13.Spacing = 10;
			w13.BorderWidth = ((uint)(5));
			w13.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonConvert = new global::Gtk.Button ();
			this.buttonConvert.CanDefault = true;
			this.buttonConvert.CanFocus = true;
			this.buttonConvert.Name = "buttonConvert";
			this.buttonConvert.UseUnderline = true;
			this.buttonConvert.Label = global::Mono.Unix.Catalog.GetString ("_Convert");
			this.AddActionWidget (this.buttonConvert, -5);
			global::Gtk.ButtonBox.ButtonBoxChild w14 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w13[this.buttonConvert]));
			w14.Expand = false;
			w14.Fill = false;
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 532;
			this.DefaultHeight = 292;
			this.Show ();
		}
	}
}
