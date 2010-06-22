using System;
using MonoDevelop.Projects;
using MonoDevelop.Projects.Dom;
using MonoDevelop.Ide.Gui.Pads.ProjectPad;
using MonoDevelop.Components.Commands;
using MonoDevelop.GtkCore.GuiBuilder;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Ide;

namespace MonoDevelop.GtkCore.NodeBuilders
{
	public class ProjectFileNodeBuilderExtension : NodeBuilderExtension
	{
		public override bool CanBuildNode (Type dataType)
		{
			return typeof(ProjectFile).IsAssignableFrom (dataType);
		}
		
		public override Type CommandHandlerType {
			get { return typeof (ComponentComponentHandler); }
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
				string className = pf.GetComponentClassName ();
				GtkDesignInfo info = GtkDesignInfo.FromProject (pf.Project);
				GuiBuilderWindow win = info.GuiBuilderProject.GetWindowForClass (className);
				
				if (win != null) {
					if (win.RootWidget.IsWindow)
						icon = ImageService.GetPixbuf ("md-gtkcore-dialog", Gtk.IconSize.Menu);
					else
						icon = ImageService.GetPixbuf ("md-gtkcore-widget", Gtk.IconSize.Menu);
				}
			}
		}
		
		public override bool HasChildNodes (ITreeBuilder builder, object dataObject)
		{
			return base.HasChildNodes (builder, dataObject);
		}
	}
	
	public class ComponentComponentHandler : ProjectFileNodeCommandHandler
	{
		
		public override void ActivateItem ()
		{
			ProjectFile pf = (ProjectFile) CurrentNode.DataItem;
			
			if (pf.IsComponentFile ()) {
				Document doc = IdeApp.Workbench.OpenDocument (pf.FilePath, true);
				if (doc != null) {
					GuiBuilderView view = doc.GetContent<GuiBuilderView> ();
					if (view != null)
						view.ShowDesignerView ();
				}
				return;
			}
					
			base.ActivateItem ();
		}
	}
}