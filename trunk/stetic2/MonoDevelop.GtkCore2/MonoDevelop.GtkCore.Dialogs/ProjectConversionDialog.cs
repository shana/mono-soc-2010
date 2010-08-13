using System;
using Gtk;

using MonoDevelop.Projects;

namespace MonoDevelop.GtkCore.Dialogs
{
	public partial class ProjectConversionDialog : Gtk.Dialog
	{		
		public ProjectConversionDialog (IntPtr raw)
			: base (raw)
		{
		}
		
		public string GuiFolderName { get; private set; }
		
		public bool MakeBackup { get; private set; }
		
		
		public ProjectConversionDialog (Project project, string guiFolderName)
		{
			this.Build ();
			
			labelProject.LabelProp = string.Format ("<b><big>{0}</big></b>", project.Name);
			entryFolder.Text = guiFolderName;
			entryFolder.Position = -1;
			
			buttonConvert.Clicked += HandleButtonConvertClicked;
		}

		void HandleButtonConvertClicked (object sender, EventArgs e)
		{
			GuiFolderName = entryFolder.Text;
			MakeBackup = checkBackup.Active;
				
			Respond (ResponseType.Yes);
		}
	}
}

