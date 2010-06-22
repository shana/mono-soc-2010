using System;
using MonoDevelop.Projects;

namespace MonoDevelop.GtkCore.NodeBuilders
{
	public static class ProjectFileExtension
	{
		public static bool IsComponentFile (this ProjectFile pf)
		{
			return pf.GetComponentClassName() != null;
		}
		
		public static string GetComponentClassName (this ProjectFile pf)
		{
			foreach (ProjectFile dependent in pf.DependentChildren)
				if (dependent.FilePath.Extension == ".gtkx")
					return dependent.FilePath.FileNameWithoutExtension;
			
			return null;
		}
	}
}

