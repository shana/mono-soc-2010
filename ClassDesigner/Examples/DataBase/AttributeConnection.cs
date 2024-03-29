//
// MonoHotDraw. Diagramming library
//
// Authors:
//	Mario Carrión <mario@monouml.org>
//
// Copyright (C) 2006, 2007, 2008 MonoUML Team (http://www.monouml.org)
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
//
using MonoHotDraw;
using MonoHotDraw.Figures;
using System;
using System.Runtime.Serialization;

namespace MonoHotDraw.Database {

	[Serializable]
	public class AttributeConnection : LineConnectionFigure {
	
		public AttributeConnection () : base () {
		}

		public AttributeConnection (IFigure fig1, IFigure fig2) : base (fig1, fig2) {
		}
		
		protected AttributeConnection (SerializationInfo info, StreamingContext context) : base (info, context) {
		}

		public override bool CanConnectEnd (IFigure figure) {
			if (figure is Attribute) {
				if (!figure.Includes (StartFigure)) {					
					return true;
				}
			}
			return false;
		}
		
		public override bool CanConnectStart (IFigure figure) {
			if (figure is Attribute) {
				if (!figure.Includes (EndFigure)) {
					return true;
				}
			}
			return false;
		}
	}
}
