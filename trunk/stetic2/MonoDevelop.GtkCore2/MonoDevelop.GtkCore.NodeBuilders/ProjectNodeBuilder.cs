//
// ProjectNodeBuilder.cs
//
// Author:
//   Lluis Sanchez Gual
//
// Copyright (C) 2006 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections;
using System.IO;

using Gtk;

using MonoDevelop.Projects;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui.Pads;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Components;

using MonoDevelop.GtkCore.Dialogs;
using MonoDevelop.GtkCore.GuiBuilder;
using MonoDevelop.Ide;

namespace MonoDevelop.GtkCore.NodeBuilders
{
	public class ProjectNodeBuilder: NodeBuilderExtension
	{
		static ProjectNodeBuilder instance;

		public override bool CanBuildNode (Type dataType)
		{
			return typeof(DotNetProject).IsAssignableFrom (dataType);
		}
		
		protected override void Initialize ()
		{
			lock (typeof (ProjectNodeBuilder))
				instance = this;
		}
		
		public override void Dispose ()
		{
			lock (typeof (ProjectNodeBuilder))
				instance = null;
		}
		
		public override void BuildNode (ITreeBuilder treeBuilder, object dataObject, ref string label, ref Gdk.Pixbuf icon, ref Gdk.Pixbuf closedIcon)
		{
			Project project = dataObject as Project;
			
			if (project is DotNetProject) {
				GtkDesignInfo info = GtkDesignInfo.FromProject (project);
				
				if (info.OldVersion) {
					ProjectConversionDialog dialog = new ProjectConversionDialog (project);
					
					try
					{
						if (dialog.Run () == (int)ResponseType.Yes) {
							info.GuiBuilderProject.Convert (dialog.GuiFolderName, dialog.MakeBackup);
							IdeApp.ProjectOperations.Save (project);
						}
					} finally {
						dialog.Destroy ();
					}
				}
				
				project.FileAddedToProject += HandleProjectFileAddedToProject;
				//project.f
			}
		}
		
		void HandleProjectFileAddedToProject (object sender, ProjectFileEventArgs e)
		{
			Project project = e.Project;
			ProjectFile pf = e.ProjectFile;
			
			string path = pf.FilePath.ToString().Replace(".cs",string.Empty) +".gtkx";
			
			if (!project.IsFileInProject(path) && File.Exists (path)) {
				ProjectFile pf2=project.AddFile(path);
				pf2.DependsOn = pf.FilePath.FileName;
			}
			
			string path2 = pf.FilePath.ToString().Replace(".cs", string.Empty) +".generated.cs";
			
			if (!project.IsFileInProject(path2) && File.Exists (path2)) {
				ProjectFile pf3=project.AddFile(path2);
				pf3.DependsOn = pf.FilePath.FileName;
			}
				
			//project.FileAddedToProject -= HandleProjectFileAddedToProject;
			
//			if (e.ProjectFile.IsComponentFile ()) {
				//GtkDesignInfo info = GtkDesignInfo.FromProject (project);
				//info.UpdateGtkFolder ();
				
				//IdeApp.ProjectOperations.Save (e.Project);
//			}
			
			//project.FileAddedToProject += HandleProjectFileAddedToProject;
			
		}
		
		public override void BuildChildNodes (ITreeBuilder builder, object dataObject)
		{
			if (GtkDesignInfo.HasDesignedObjects ((Project)dataObject))
				builder.AddChild (new WindowsFolder ((Project)dataObject));
		}
		
		public static void OnSupportChanged (Project p)
		{
			if (instance == null)
				return;

			ITreeBuilder tb = instance.Context.GetTreeBuilder (p);
			if (tb != null)
				tb.UpdateAll ();
		}
	}
}
