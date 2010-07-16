using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mono.CodeContracts.CCRewrite {
	public class RewriterOptions {

		public RewriterOptions ()
		{
			// Initialise to defaults
			this.Assembly = null;
			this.Debug = true;
			this.Level = 4;
			this.WritePdbFile = true;
			this.Rewrite = true;
			this.BreakIntoDebugger = false;
			this.ThrowOnFailure = false;
			this.OutputFile = null;
		}

		public string Assembly { get; set; }
		public bool Debug { get; set; }
		public int Level { get; set; }
		public bool WritePdbFile { get; set; }
		public bool Rewrite { get; set; }
		public bool BreakIntoDebugger { get; set; }
		public bool ThrowOnFailure { get; set; }
		public string OutputFile { get; set; }

	}
}
