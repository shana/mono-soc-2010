using System;
using MonoDevelop.GtkCore.GuiBuilder;
using MonoDevelop.Projects;

namespace MonoDevelop.GtkCore.NodeBuilders
{
	public enum GtkComponentType 
	{
		Dialog,
		Widget,
		ActionGroup,
		Unknown
	}
	
	public static class ProjectFileExtension
	{
		public static bool IsComponentFile (this ProjectFile pf)
		{
			return pf.GetComponentClassName() != null;
		}
		
		public static string GetComponentClassName (this ProjectFile pf)
		{
			foreach (ProjectFile dependent in pf.DependentChildren)
				if (dependent.FilePath.Extension == ".gtkx") {
					GtkDesignInfo info = GtkDesignInfo.FromProject (pf.Project);
					return info.GuiBuilderProject.GetClassNameForGtkxFile (dependent.FilePath.ToString ());
				}
			
			return null;
		}
		
		public static GtkComponentType GetComponentType (this ProjectFile pf)
		{
			GtkDesignInfo info = GtkDesignInfo.FromProject (pf.Project);
			string className = pf.GetComponentClassName ();
			
			if (className != null) {
				GuiBuilderWindow win = info.GuiBuilderProject.GetWindowForClass (className);
				if (win != null) 
						return win.RootWidget.IsWindow ? GtkComponentType.Dialog : GtkComponentType.Widget;
							
				Stetic.ActionGroupInfo action =	info.GuiBuilderProject.GetActionGroup (className);
				if (action != null)
					return GtkComponentType.ActionGroup;
			}
			
			return GtkComponentType.Unknown;
		}
	}
}

