// MonoHotDraw. Diagramming Framework
//
// Authors:
//	Manuel Cerón <ceronman@gmail.com>
//	Mario Carrión <mario@monouml.org>
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

using System.Collections;
using System.Collections.Generic;
using MonoHotDraw.Commands;
using MonoHotDraw.Util;

namespace MonoHotDraw.Figures
{
	public class FigureCollection : List<IFigure>, System.ICloneable
	{
		public FigureCollection () : base ()
		{
		}

		public FigureCollection (IEnumerable <IFigure> list) : base (list)
		{
		}
		
		#region Public Api
		public RectangleD GetBounds ()
		{
			var rectangle = new RectangleD (0, 0, 0, 0);
			
			foreach (IFigure figure in this)
				rectangle.Add (figure.DisplayBox);
			
			return rectangle;
		}
		#endregion

		#region ICloneable implementation
		object System.ICloneable.Clone ()
		{
			return GenericCloner.Clone<FigureCollection> (this); 
		}
		
		public FigureCollection Clone ()
		{
			return (FigureCollection) GenericCloner.Clone<FigureCollection> (this); 
		}
		#endregion		
	}
	
	public static class FigureCollectionExtensions
	{
		public static FigureCollection ToFigures (this IEnumerable<IFigure> collection)
	    {
	          return new FigureCollection (collection);
	    }
	}
}