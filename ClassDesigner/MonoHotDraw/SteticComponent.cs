// MonoHotDraw. Diagramming Framework
//
// Authors:
//	Manuel Cerón <ceronman@gmail.com>
//
// Copyright (C) 2006, 2007, 2008, 2009 MonoUML Team (http://www.monouml.org)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using Gtk;

using MonoHotDraw.Commands;
using MonoHotDraw.Tools;
using MonoHotDraw.Figures;

namespace MonoHotDraw
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class SteticComponent : Gtk.Bin, IDrawingEditor
	{
		ITool _tool;
		
		public SteticComponent()
		{
			this.Build();
			
		
			View = new StandardDrawingView (this);
			this.scrolledwindow.Add ((Widget) View);
			Tool = new SelectionTool (this);
			CommandManager = CommandManager.CreateInstance (this);
			UndoManager = new UndoManager();
			UndoManager.StackChanged += delegate {
				OnUndoStackChanged();
			};
		}
		
		public event EventHandler UndoStackChanged;
		
		public IDrawingView View { get; set; }
		public CommandManager CommandManager { get; private set; }
		public UndoManager UndoManager { get; private set; }
		
		public virtual void DisplayMenu (IFigure figure, MouseEvent ev)
		{
		}
		
		public ITool Tool {
			get { return _tool; }
			set {
				if (_tool != null && _tool.Activated)
					_tool.Deactivate();
				
				_tool = value;
				if (value != null)
					_tool.Activate();
			}
		}
		
		public void Undo ()
		{
			var command = new UndoCommand("Undo", this);
			command.Execute();
		}
		
		public void Redo ()
		{
			var command = new RedoCommand("Redo", this);
			command.Execute();
		}
		
		public void AddWithDragging (IFigure figure)
		{
			Tool = new DragCreationTool(this, figure);
		}
		
		public void AddWithResizing (IFigure figure)
		{
			Tool = new ResizeCreationTool(this, figure);
		}
		
		public void AddConnection (IConnectionFigure figure)
		{
			Tool = new ConnectionCreationTool(this, figure);
		}
		
		protected void OnUndoStackChanged()
		{
			if (UndoStackChanged != null)
				UndoStackChanged(this, EventArgs.Empty);
		}
	}
}
