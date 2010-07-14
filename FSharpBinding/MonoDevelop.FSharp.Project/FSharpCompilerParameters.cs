//
// FSharpCompilerParameters.cs
//
// Author:
//   Vasili I Galchin <vigalchin@gmail.com> but heavily based on CSharp implementation by Mike Krüger <mkrueger@novell.com>
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
using System.Xml;
using System.Diagnostics;
using System.ComponentModel;
// using MonoDevelop.Core.Gui.Components;
using MonoDevelop.Projects;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.FSharp.Project
{
	/// <summary>
	/// This class handles project specific compiler parameters
	/// </summary>
	public class FSharpCompilerParameters: ConfigurationParameters
	{
		public FSharpCompilerParameters(): base() {
		}
		// Configuration parameters
		
		[ItemProperty ("WarningLevel")]
		int  warninglevel = 4;
		
		[ItemProperty ("NoWarn", DefaultValue = "")]
		string noWarnings = String.Empty;
		
		[ItemProperty ("Optimize")]
		bool optimize;

		[ItemProperty("Tailcalls")]
		bool tailcalls;

		[ItemProperty("CrossOptimize")]
		bool crossoptimize;
		
		[ItemProperty ("AllowUnsafeBlocks", DefaultValue = false)]
		bool unsafecode = false;
		
		[ItemProperty ("DefineConstants", DefaultValue = "")]
		string definesymbols = String.Empty;
		
		[ItemProperty ("GenerateDocumentation", DefaultValue = false)]
		bool generateXmlDocumentation = false;
		
		[ItemProperty ("additionalargs", DefaultValue = "")]
		string additionalArgs = string.Empty;
		
		[ItemProperty ("TreatWarningsAsErrors", DefaultValue = false)]
		bool treatWarningsAsErrors;

		[ItemProperty("PlatformTarget", DefaultValue="anycpu")]
		string platformTarget = "anycpu";
		
		[ItemProperty("WarningsNotAsErrors", DefaultValue="")]
		string warningsNotAsErrors = "";
		
		
		#region Members required for backwards compatibility. Not used for anything else.
		
		[ItemProperty ("StartupObject", DefaultValue = null)]
		internal string mainclass;

		[ProjectPathItemProperty ("Win32Resource", DefaultValue = null)]
		internal string win32Resource;
	
		[ItemProperty ("CodePage", DefaultValue = null)]
		internal string codePage;
		
		#endregion
		
		
		protected override void OnEndLoad ()
		{
			base.OnEndLoad ();
			
			// Backwards compatibility. Move parameters to the project parameters object
			if (ParentConfiguration != null && ParentConfiguration.ProjectParameters != null) {
				FSharpProjectParameters cparams = (FSharpProjectParameters) ParentConfiguration.ProjectParameters;
				if (win32Resource != null) {
					cparams.Win32Resource = win32Resource;
					win32Resource = null;
				}
				if (mainclass != null) {
					cparams.MainClass = mainclass;
					mainclass = null;
				}
				if (!string.IsNullOrEmpty (codePage)) {
					cparams.CodePage = int.Parse (codePage);
					codePage = null;
				}
			}
		}
	
		public string AdditionalArguments {
			get { return additionalArgs; }
			set { additionalArgs = value ?? string.Empty; }
		}

#region Code Generation
		
		public string DefineSymbols {
			get {
				return definesymbols;
			}
			set {
				definesymbols = value ?? string.Empty;
			}
		}
		
		public bool Optimize {
			get {
				return optimize;
			}
			set {
				optimize = value;
			}
		}

		public bool CrossOptimize {
			get {
				return crossoptimize;
			}
			set { crossoptimize = value; }
		}

		public bool TailCalls {
			get {
				return tailcalls;
			}
			set {
				tailcalls = value;
			}
		}
		
		public bool UnsafeCode {
			get {
				return unsafecode;
			}
			set {
				unsafecode = value;
			}
		}
		
		public bool GenerateXmlDocumentation {
			get {
				return generateXmlDocumentation;
			}
			set {
				generateXmlDocumentation = value;
			}
		}
		
		public string PlatformTarget {
			get {
				return platformTarget;
			}
			set {
				platformTarget = value ?? string.Empty;
			}
		}
		
#endregion

#region Errors and Warnings 
		public int WarningLevel {
			get {
				return warninglevel;
			}
			set {
				warninglevel = value;
			}
		}
		
		public string NoWarnings {
			get {
				return noWarnings;
			}
			set {
				noWarnings = value;
			}
		}

		public bool TreatWarningsAsErrors {
			get {
				return treatWarningsAsErrors;
			}
			set {
				treatWarningsAsErrors = value;
			}
		}
		
		public string WarningsNotAsErrors {
			get {
				return warningsNotAsErrors;
			}
			set {
				warningsNotAsErrors = value;
			}
		}
#endregion
	}
}
