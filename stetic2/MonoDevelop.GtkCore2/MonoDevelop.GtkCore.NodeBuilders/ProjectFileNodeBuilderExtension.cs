using System;

using MonoDevelop.Components.Commands;
using MonoDevelop.GtkCore.GuiBuilder;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Commands;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Ide.Gui.Pads.ProjectPad;
using MonoDevelop.Projects;
using MonoDevelop.Projects.Dom;

namespace MonoDevelop.GtkCore.NodeBuilders
{
	public class ProjectFileBuilder : ProjectFileNodeBuilder 
	{
	
	}
	
	public class ProjectFileNodeBuilderExtension : NodeBuilderExtension
	{
		public override bool CanBuildNode (Type dataType)
		{
			return typeof(ProjectFile).IsAssignableFrom (dataType);
		}
		
		public override Type CommandHandlerType {
			get { return typeof (ComponentCommandHandler); }
		}
		
		public override void GetNodeAttributes (ITreeNavigator treeNavigator, object dataObject, ref NodeAttributes attributes)
		{
			if (treeNavigator.Options ["ShowAllFiles"])
				return;
			
			ProjectFile pf = (ProjectFile) dataObject;
			if (pf.FilePath.Extension == ".gtkx")
				attributes |= NodeAttributes.Hidden;
		}
		
		
		public override void BuildNode (ITreeBuilder treeBuilder, object dataObject, ref string label, ref Gdk.Pixbuf icon, ref Gdk.Pixbuf closedIcon)
		{	
			ProjectFile pf = (ProjectFile) dataObject;
			
			if (pf.IsComponentFile ()) {
				GtkComponentType type = pf.GetComponentType ();
				
				switch (type) {
				case GtkComponentType.Dialog : 
					icon = ImageService.GetPixbuf ("md-gtkcore-dialog", Gtk.IconSize.Menu);
					break;
				case GtkComponentType.Widget :
					icon = ImageService.GetPixbuf ("md-gtkcore-widget", Gtk.IconSize.Menu);
					break;
				case GtkComponentType.ActionGroup :
					icon = ImageService.GetPixbuf ("md-gtkcore-actiongroup", Gtk.IconSize.Menu);
					break;
				}	
			}
		}
		
		public override bool HasChildNodes (ITreeBuilder builder, object dataObject)
		{
			return base.HasChildNodes (builder, dataObject);
		}
	}
	
	public class ComponentCommandHandler : ProjectFileNodeCommandHandler
	{
		public override void ActivateItem ()
		{
			ProjectFile pf = (ProjectFile) CurrentNode.DataItem;
			
			if (pf.IsComponentFile ()) {
				Document doc = IdeApp.Workbench.OpenDocument (pf.FilePath, true);
				
				if (doc != null) {
					GuiBuilderView view = doc.GetContent<GuiBuilderView> ();
					if (view != null) {
						GtkComponentType type = pf.GetComponentType ();
				
						switch (type) {
						case GtkComponentType.Dialog : 
						case GtkComponentType.Widget :
							view.ShowDesignerView ();
							break;
						case GtkComponentType.ActionGroup :
							view.ShowActionDesignerView (((Stetic.ActionGroupInfo) CurrentNode.DataItem).Name);
							break;
						}
					}
				}
				return;	
			}
			base.ActivateItem ();
		}

	}
}