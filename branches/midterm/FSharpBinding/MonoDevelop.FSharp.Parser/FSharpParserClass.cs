using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoDevelop.Projects.Dom.Parser;
using System.IO;
using MonoDevelop.Projects.Dom;

namespace FSharpBinding.MonoDevelop.FSharp.Parser {
	public class FSharpParser: AbstractParser {
		public override bool CanParse(string fileName)
		{
			return Path.GetExtension(fileName) == ".fs";
		}

		public FSharpParser(): base("F#", "text/x-fsharp")
		{
		}

		public override ParsedDocument Parse(ProjectDom dom, string fileName, string content)
		{
			ParsedDocument result = new ParsedDocument(fileName);
			result.CompilationUnit = new CompilationUnit(fileName);
		}
	}
}
