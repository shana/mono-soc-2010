// 
// SyntaxMode.cs
//  
// Author:
//   Mike Krüger <mkrueger@novell.com>
//
// Copyright (C) 2009 Novell, Inc (http://www.novell.com)
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
using System.Linq;
using System.Collections.Generic;
using Mono.TextEditor.Highlighting;
using Mono.TextEditor;
using System.Xml;
using MonoDevelop.Projects;
using MonoDevelop.FSharp.Project;
using MonoDevelop.Core;
using MonoDevelop.Projects.Dom.Parser;
using MonoDevelop.Projects.Dom;
using MonoDevelop.Ide;

namespace MonoDevelop.FSharp.Highlighting
{
	public class FSharpSyntaxMode : Mono.TextEditor.Highlighting.SyntaxMode
	{
		public bool DisableConditionalHighlighting {
			get;
			set;
		}
		
		static FSharpSyntaxMode ()
		{
			MonoDevelop.Debugger.DebuggingService.DisableConditionalCompilation += (EventHandler<MonoDevelop.Ide.Gui.DocumentEventArgs>)DispatchService.GuiDispatch (new EventHandler<MonoDevelop.Ide.Gui.DocumentEventArgs>(OnDisableConditionalCompilation));
		}
		
		static void OnDisableConditionalCompilation (object s, MonoDevelop.Ide.Gui.DocumentEventArgs e)
		{
			ITextEditorDataProvider provider = e.Document.GetContent<ITextEditorDataProvider> ();
			if (provider == null)
				return;
			FSharpSyntaxMode mode = provider.GetTextEditorData ().Document.SyntaxMode as FSharpSyntaxMode;
			if (mode == null)
				return;
			mode.DisableConditionalHighlighting = true;
			provider.GetTextEditorData ().Document.CommitUpdateAll ();
		}
		
		public FSharpSyntaxMode ()
		{
			ResourceXmlProvider provider = new ResourceXmlProvider (typeof(FSharpSyntaxMode).Assembly, "FSharpSyntaxMode.xml");
			using (XmlReader reader = provider.Open ()) {
				SyntaxMode baseMode = SyntaxMode.Read (reader);
				this.rules = new List<Rule> (baseMode.Rules);
				this.keywords = new List<Keywords> (baseMode.Keywords);
				this.spans = baseMode.Spans;
				this.matches = baseMode.Matches;
				this.prevMarker = baseMode.PrevMarker;
				this.SemanticRules = new List<SemanticRule> (baseMode.SemanticRules);
				this.table = baseMode.Table;
				this.properties = baseMode.Properties;
			}
			
			AddSemanticRule ("Comment", new HighlightUrlSemanticRule ("comment"));
			AddSemanticRule ("XmlDocumentation", new HighlightUrlSemanticRule ("comment"));
			AddSemanticRule ("String", new HighlightUrlSemanticRule ("string"));
		}
		
		//public override SpanParser CreateSpanParser (Document doc, SyntaxMode mode, LineSegment line, Stack<Span> spanStack)
		//{
		//    return new FSharpSpanParser (doc, mode, line, spanStack);
		//}
		
		class IfBlockSpan : Span
		{
			public bool IsValid {
				get;
				private set;
			}
			
			public IfBlockSpan (bool isValid)
			{
				this.IsValid = isValid;
				TagColor = "text.preprocessor";
				if (!isValid) {
					Color = "comment.block";
					Rule = "String";
				} else {
					Color = "text";
					Rule = "<root>";
				}
				StopAtEol = false;
			}
			public override string ToString ()
			{
				return string.Format("[IfBlockSpan: IsValid={0}, Color={1}, Rule={2}]", IsValid, Color, Rule);
			}
		}
		
		class ElseIfBlockSpan : Span
		{
			public bool IsValid {
				get;
				private set;
			}
			
			public ElseIfBlockSpan (bool isValid)
			{
				this.IsValid = isValid;
				TagColor = "text.preprocessor";
				if (!isValid) {
					Color = "comment.block";
					Rule = "String";
				} else {
					Color = "text";
					Rule = "<root>";
				}
				StopAtEol = false;
			}
			public override string ToString ()
			{
				return string.Format("[ElseIfBlockSpan: IsValid={0}, Color={1}, Rule={2}]", IsValid, Color, Rule);
			}
		}
		
		class ElseBlockSpan : Span
		{
			public bool IsValid {
				get;
				private set;
			}
			
			public ElseBlockSpan (bool isValid)
			{
				this.IsValid = isValid;
				TagColor = "text.preprocessor";
				if (!isValid) {
					Color = "comment.block";
					Rule = "String";
				} else {
					Color = "text";
					Rule = "<root>";
				}
				StopAtEol = false;
			}
			
			public override string ToString ()
			{
				return string.Format("[ElseBlockSpan: IsValid={0}, Color={1}, Rule={2}]", IsValid, Color, Rule);
			}
		}
	}
}
 
