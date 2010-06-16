//
// ProjectFolderNodeBuilderExtension.cs
//
// Author:
//   Krzysztof Marecki
//
// Copyright (C) 2010 Novell, Inc (http://www.novell.com)
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
using MonoDevelop.Projects;
using MonoDevelop.Ide.Gui.Pads.ProjectPad;
using MonoDevelop.Components.Commands;
using MonoDevelop.GtkCore.GuiBuilder;
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
		
		public override void GetNodeAttributes (ITreeNavigator parentNode, object dataObject, ref NodeAttributes attributes)
		{
		
		}
		
		
		public override void BuildNode (ITreeBuilder treeBuilder, object dataObject, ref string label, ref Gdk.Pixbuf icon, ref Gdk.Pixbuf closedIcon)
		{	
			ProjectFile pf = (ProjectFile) dataObject;
			GtkDesignInfo info = GtkDesignInfo.FromProject (pf.Project);
			GuiBuilderWindow win = info.GuiBuilderProject.GetWindowForFile (pf.FilePath.FullPath);
			
			if (win != null) {
				if (win.RootWidget.IsWindow)
					icon = ImageService.GetPixbuf ("md-gtkcore-dialog", Gtk.IconSize.Menu);
				else
					icon = ImageService.GetPixbuf ("md-gtkcore-widget", Gtk.IconSize.Menu);
			}
		}
		
		public override bool HasChildNodes (ITreeBuilder builder, object dataObject)
		{
			return base.HasChildNodes (builder, dataObject);
		}
		
		
		public override void BuildChildNodes (ITreeBuilder treeBuilder, object dataObject)
		{
//			ProjectFile file = (ProjectFile) dataObject;
//			GtkDesignInfo info = GtkDesignInfo.FromProject (file.Project);
			
			//file.ExtendedProperties.
		}
		
		
		
	}
}