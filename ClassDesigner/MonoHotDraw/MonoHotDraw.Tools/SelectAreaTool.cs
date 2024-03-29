// TODO: RENAME
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

using Cairo;
using Gdk;
using MonoHotDraw.Figures;
using MonoHotDraw.Util;

namespace MonoHotDraw.Tools
{
	public class SelectAreaTool : AbstractTool
	{
		public SelectAreaTool (IDrawingEditor editor) : base (editor)
		{
		}
		
		#region Mouse Events
		public override void MouseDown (MouseEvent ev)
		{
			base.MouseDown (ev);
			
			var view = ev.View;
			var gdk_event = (EventButton) ev.GdkEvent;

			bool shift_pressed = (gdk_event.State & ModifierType.ShiftMask) != 0;
			
			if (!shift_pressed)
				view.ClearSelection ();

			selectionRectangle = new RectangleD (ev.X, ev.Y, 0, 0);
			DrawSelectionRect ((Gtk.Widget) view, gdk_event.Window);
			
		}

		public override void MouseUp (MouseEvent ev)
		{
			var view = ev.View;
			var gdk_event = (EventButton) ev.GdkEvent;
			
			DrawSelectionRect ((Gtk.Widget) view, gdk_event.Window);
			
			bool shift_pressed = (gdk_event.State & ModifierType.ShiftMask) != 0;
			
			SelectFiguresOnRect (view, shift_pressed);
		}

		public override void MouseDrag (MouseEvent ev)
		{
			DrawSelectionRect ((Gtk.Widget) ev.View, ev.GdkEvent.Window);
			
			var anchor = new PointD (AnchorX, AnchorY);
			var corner = new PointD (ev.X, ev.Y);
		
			selectionRectangle = new RectangleD (anchor, corner);
			DrawSelectionRect ((Gtk.Widget) ev.View, ev.GdkEvent.Window);
		}
		#endregion
		
		#region Selection Area Tool Members
		private void DrawSelectionRect (Gtk.Widget widget, Gdk.Window window)
		{
			Gdk.GC gc = widget.Style.WhiteGC;
			gc.SetLineAttributes (1, LineStyle.OnOffDash, CapStyle.Butt, JoinStyle.Miter);
			gc.Function = Function.Xor;
			selectionRectangle.Normalize ();
			
			RectangleD r = selectionRectangle;
			PointD p = View.DrawingToView(r.X, r.Y);
			
			window.DrawRectangle (gc, false, (int) p.X,
			                       (int) p.Y,
			                       (int) (r.Width * View.Scale),
			                       (int) (r.Height * View.Scale));
		}
		
		private void SelectFiguresOnRect (IDrawingView view, bool shift_pressed)
		{
			foreach (IFigure figure in view.Drawing.FiguresReversed) {
				RectangleD rect = figure.DisplayBox;
				
				if (selectionRectangle.Contains (rect.X, rect.Y) &&
					selectionRectangle.Contains (rect.X2, rect.Y2)) {
						if (shift_pressed)
							view.ToggleSelection (figure);
						else
							view.AddToSelection (figure);
				}
			}
		}
		
		private RectangleD selectionRectangle;
		#endregion
	}
}
