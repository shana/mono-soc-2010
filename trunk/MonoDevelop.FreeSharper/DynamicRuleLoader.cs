// 
// DynamicRuleLoader.cs
//  
// Author:
//       Nikhil Sarda <diff.operator@gmail.com>
// 
// Copyright (c) 2010 Nikhil Sarda
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
using System.IO;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.CSharp;

using MonoDevelop.Projects.Dom;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Dialogs;
using MonoDevelop.Projects.Dom.Parser;

using Mono.Addins;

namespace MonoDevelop.FreeSharper
{
	public class DynamicRuleLoader 
	{
		private static DynamicRuleLoader instance = new DynamicRuleLoader();
		
		public static DynamicRuleLoader Instance {
			get {
				return instance;
			}
		}
		
		public CSharpCodeProvider CsCodeProvider {
			get; private set;
		}
		
		public CodeCompiler CSCodeCompiler {
			get; private set;
		}
		
		public CompilerParameters CompilerParams {
			get; private set;
		}
		
		public CompilerResults DRLCompilerResults {
			get; private set;
		}
		
		private FileStream sourceFile;
		private Document fileDocument;
		
		private OpenFileDialog fileChooserDialog;
		
		public void SelectFile()
		{
			fileChooserDialog = new OpenFileDialog();
			fileChooserDialog.Title = "Select rule to load";
			fileChooserDialog.SelectMultiple = false; //We do not allow multiple files to be loaded for now
			fileChooserDialog.Action = Gtk.FileChooserAction.Open;
			fileChooserDialog.Run();
		}
		
		private DynamicRuleLoader ()
		{
			CsCodeProvider = new CSharpCodeProvider();
			//CSCodeCompiler = new CodeCompiler();
			CompilerParams = new CompilerParameters();
		}
		
		private void LoadFile() 
		{
			//fileChooser = new FileSelectorDialog();
			//TODO logic to grab file from filechooser
		}
		
		private void CreateAssembly()
		{
			
		}
	}
}

