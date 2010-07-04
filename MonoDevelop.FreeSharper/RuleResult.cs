// 
// RuleResult.cs
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
using System.Collections.Generic;
using System.Text;

using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;
using MonoDevelop.Projects;

namespace MonoDevelop.FreeSharper
{
	public class RuleResult
	{
		[ItemProperty]
		FilePath resultDocFilePath;
		
		[ItemProperty (DefaultValue = 0)]
		int line;
		
		[ItemProperty (DefaultValue = 0)]
		int col;
		
		[ItemProperty (DefaultValue = "")]
		string description = string.Empty;
		
		[ItemProperty (DefaultValue = "")]
		string id = string.Empty;
		
		[ItemProperty (DefaultValue = ResultMessageType.Default)]
		ResultMessageType resultType = ResultMessageType.Default;
		
		[ItemProperty (DefaultValue = false)]
		bool stale;
	
		public ResultMessageType ResultMessageType {
			get {
				return resultType;
			}
			private set {
				resultType = value;
			}
		}
		
		public int Line {
			get {
				return line;
			}
			internal set {
				line = value;
			}
		}
		
		public int Col {
			get {
				return col;
			}
			internal set {
				col = value;
			}
		}
		
		public string Message {
			get {
				return description;
			}
			private set {
				description = value;
			}
		}
		
		public string ResultID {
			get {
				return id;
			}
		}
		
		public Document ResultDoc {
			get; private set;
		}
		
		public bool IsStale {
			get {
				return stale;
			}
			internal set {
				stale = value;
			}
		}
		
		//TODO Add stuff about quick fix options, ie how quick fixes will be shown
				
		public RuleResult (ResultMessageType msgType, int line, int col, string msg, Document doc)
		{
			if(msgType == null)
				this.ResultMessageType = ResultMessageType.Default;
			else 
				this.ResultMessageType = msgType;
			this.Line = line;
			this.Col = col;
			this.Message = msg;
			this.ResultDoc = doc;
			
			resultDocFilePath = this.ResultDoc.FileName;
			SetResultMoniker ();
		}
		
		public void SetResultMoniker()
		{
			StringBuilder text = new StringBuilder("");
			text.Append("result://");
			
			switch(this.resultType) {
			case ResultMessageType.Error:
				text.Append("error/");
				break;
			case ResultMessageType.Warning:
				text.Append("warning/");
				break;
			case ResultMessageType.Convention:
				text.Append("convention/");
				break;
			case ResultMessageType.Suggestion:
				text.Append("suggestion/");
				break;
			default:
				break;
			}
			
			text.Append(line.ToString() + "/" + col.ToString());
			text.Append("/" + this.Message);
			text.Append("/" + this.ResultDoc.FileName.FileName);
			this.id = text.ToString();
		}
		
		public override string ToString ()
		{
			return string.Format("[RuleResult: ResultMessageType={0}, LineToMark={1}, Message={2}, ResultID={3}, ResultDoc={4}, IsStale={5}]", ResultMessageType, Line, Message, ResultID, ResultDoc, IsStale);
		}
		
		
		public void Dispose()
		{
		}
		
		public virtual void JumpToPosition()
		{
			if (ResultDoc != null) {
				if(IdeApp.Workbench.ActiveDocument != ResultDoc)
					IdeApp.Workbench.OpenDocument(ResultDoc.FileName);
				IdeApp.Workbench.ActiveDocument.TextEditor.JumpTo(this.Line, 1);
			} else {
				MessageService.ShowError("File does not exist!");
				this.stale = true;
			}
		}
		
		public string GetPath ()
		{
			if (this.ResultDoc.Project != null)
				return FileService.AbsoluteToRelativePath (this.ResultDoc.Project.BaseDirectory, this.ResultDoc.FileName);
			
			return this.ResultDoc.FileName;
		}
		
		public string GetProject ()
		{
			return (this.ResultDoc.Project is IWorkspaceObject)? this.ResultDoc.Project.Name: string.Empty;
		}
	}
}