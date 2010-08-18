//
// CodeBinder.cs
//
// Author:
//   Lluis Sanchez Gual
//
// Copyright (C) 2006 Novell, Inc (http://www.novell.com)
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


using System.CodeDom;
using System.Collections;

using MonoDevelop.Core;
using MonoDevelop.Core.ProgressMonitoring;
using MonoDevelop.Projects;
using MonoDevelop.Projects.Text;
using MonoDevelop.Projects.Dom;
using MonoDevelop.Projects.Dom.Parser;
using MonoDevelop.Projects.CodeGeneration;
using MonoDevelop.Ide.Gui;
using MonoDevelop.GtkCore.Dialogs;
using MonoDevelop.Ide;

namespace MonoDevelop.GtkCore.GuiBuilder
{
	/// This class provides several methods for managing the relation
	/// between an object (e.g. a window) and the source code that will implement the
	/// code for that object.
	///
	/// Once created, a CodeBinder object will keep track of the class bound to the
	/// object. If the class is renamed, it will properly update the object name.

	public class CodeBinder
	{
		ITextFileProvider textFileProvider;
		Stetic.Component targetObject;
		Project project;
		GuiBuilderProject gproject;
		string className;
		string classFile;
		
		public CodeBinder (Project project, ITextFileProvider textFileProvider, Stetic.Component targetObject)
		{
			this.project = project;
			this.textFileProvider = textFileProvider;

			gproject = GtkDesignInfo.FromProject (project).GuiBuilderProject;

			TargetObject = targetObject;
		}
		
		public Stetic.Component TargetObject {
			get { return targetObject; }
			set {
				this.targetObject = value;
				if (targetObject != null) {
					IType cls = gproject.FindClass (GetClassName (targetObject));
					if (cls != null) {
						className = cls.FullName;
						classFile = cls.CompilationUnit.FileName;
					}
				}
			}
		}
		
		/// Synchronizes the bindings between the object and the source code
		public void UpdateBindings (string fileName)
		{
			if (targetObject == null)
				return;
#if TRUNK
			ParsedDocument doc = ProjectDomService.Parse (project, fileName);
#else
			ParsedDocument doc = ProjectDomService.Parse (project, fileName, null);
#endif			
			classFile = fileName;
			
			if (doc != null && doc.CompilationUnit != null) {
				IType cls = GetClass ();
				UpdateBindings (targetObject, cls);
			
				if (cls != null)
					targetObject.GeneratePublic = cls.IsPublic;
			}
		}
		
		void UpdateBindings (Stetic.Component obj, IType cls)
		{
			if (targetObject == null)
				return;

			// Remove signals for which there isn't a handler in the class
			
			Stetic.SignalCollection objectSignals = obj.GetSignals ();
			if (objectSignals != null) {
				Stetic.Signal[] signals = new Stetic.Signal [objectSignals.Count];
				objectSignals.CopyTo (signals, 0);
				
				foreach (Stetic.Signal signal in signals) {
					if (FindSignalHandler (cls, signal) == null) {
						obj.RemoveSignal (signal);
					}
				}
			}

			// Update children
			
			foreach (Stetic.Component ob in obj.GetChildren ())
				UpdateBindings (ob, cls);
		}
		
		IMethod FindSignalHandler (IType cls, Stetic.Signal signal)
		{
			foreach (IMethod met in cls.Methods) {
				if (met.Name == signal.Handler) {
					return met;
				}
			}
			return null;
		}

		public void UpdateField (Stetic.Component obj, string oldName)
		{
			if (targetObject == null)
				return;
				
			CodeRefactorer cr = GetCodeGenerator ();
			
			IType cls;
			
			if (obj == targetObject)
				return;	// The root widget name can only be changed internally.
			else
				cls = GetClass (false);
			
			string newName = GetObjectName (obj);
			if (newName.Length == 0)
				return;
			
			if (cls != null) {
				IField f = ClassUtils.FindWidgetField (cls, oldName);
				if (f != null) {
					cr.RenameMember (new NullProgressMonitor (), cls, f, newName, RefactoryScope.File);
				}
			}
		}
		
		/// Adds a signal handler to the class
		public void BindSignal (Stetic.Signal signal)
		{
			if (targetObject == null)
				return;

			IType cls = GetClass ();
			if (cls == null)
				return;
			
			if (FindSignalHandler (cls, signal) != null)
				return;

			CodeMemberMethod met = new CodeMemberMethod ();
			met.Name = signal.Handler;
			met.Attributes = MemberAttributes.Family;
			met.ReturnType = new CodeTypeReference (signal.SignalDescriptor.HandlerReturnTypeName);
			
			foreach (Stetic.ParameterDescriptor pinfo in signal.SignalDescriptor.HandlerParameters)
				met.Parameters.Add (new CodeParameterDeclarationExpression (pinfo.TypeName, pinfo.Name));
			CodeRefactorer gen = GetCodeGenerator ();
			gen.AddMember (cls, met);
		}
		
		public void UpdateSignal (Stetic.Signal oldSignal, Stetic.Signal newSignal)
		{
			if (targetObject == null)
				return;

			if (oldSignal.Handler == newSignal.Handler)
				return;

			IType cls = GetClass ();
			if (cls == null) return;

			IMethod met = FindSignalHandler (cls, oldSignal);
			if (met == null) return;
			CodeRefactorer gen = GetCodeGenerator ();
			gen.RenameMember (new NullProgressMonitor (), cls, met, newSignal.Handler, RefactoryScope.File);
		}

		/// Adds a field to the class
		public void BindToField (Stetic.Component obj)
		{
			if (targetObject == null)
				return;

			string name = GetMemberName (obj);
			IType cls = GetClass ();
			
			if (FindField (cls, name) != null)
				return;

			Document doc = IdeApp.Workbench.OpenDocument (cls.CompilationUnit.FileName, true);
			
			IEditableTextFile editor = doc.GetContent<IEditableTextFile> ();
			if (editor != null) {
				CodeRefactorer gen = GetCodeGenerator ();
				gen.AddMember (cls, GetFieldCode (obj, name));
			}
		}
		
		CodeMemberField GetFieldCode (Stetic.Component obj, string name)
		{
			string type = obj.Type.ClassName;
			CodeMemberField field = new CodeMemberField (type, name);
			field.Attributes = MemberAttributes.Family;
			return field;
		}
		
		IField FindField (IType cls, string name)
		{
			foreach (IField field in cls.Fields)
				if (field.Name == name)
					return field;
			return null;
		}
		
		CodeRefactorer GetCodeGenerator ()
		{
			CodeRefactorer cr = new CodeRefactorer (project.ParentSolution);
			cr.TextFileProvider = textFileProvider;
			return cr;
		}
		
		public IType GetClass ()
		{
			return GetClass (true);
		}
		
		public IType GetClass (bool getUserClass)
		{
			if (targetObject == null)
				return null;

			IType cls = gproject.FindClass (className, getUserClass);
			if (cls != null)
				return cls;
				
			// The class name may have changed. Try to guess the new name.
			
			ArrayList matches = new ArrayList ();
			ICompilationUnit unit = null;
			ProjectDom ctx = gproject.GetParserContext ();
#if TRUNK
			ParsedDocument doc = ProjectDomService.Parse (project, classFile);
#else
			ParsedDocument doc = ProjectDomService.Parse (project, classFile, null);
#endif
			
			if (doc != null && doc.CompilationUnit != null) {
				unit = doc.CompilationUnit;
				foreach (IType fcls in unit.Types) {
					if (IsValidClass (ctx, fcls, targetObject))
						matches.Add (fcls);
				}
			}
			
			// If found the class, just return it
			if (matches.Count == 1) {
				cls = (IType) matches [0];
				className = cls.FullName;
				targetObject.Name = className;
				gproject.Save (true);
				return cls;
			}
			
			// If not found, warn the user.
			
			if (unit != null && unit.Types.Count > 0) {
				using (SelectRenamedClassDialog dialog = new SelectRenamedClassDialog (unit.Types)) {
					if (dialog.Run ()) {
						className = dialog.SelectedClass;
						if (className == null)
							return null;
						else {
							targetObject.Name = className;
							gproject.Save (true);
							return gproject.FindClass (className);
						}
					}
				}
			} else {
				MessageService.ShowError (GettextCatalog.GetString ("The class bound to the component '{0}' could not be found. This may be due to syntax errors in the source code file.", GetObjectName(targetObject)));
			}
			
			return null;
		}
		
		static bool IsValidClass (ProjectDom ctx, IType cls, Stetic.Component obj)
		{
			if (cls.BaseTypes != null) {
				string typeName = obj.Type.ClassName;
				
				foreach (IReturnType bt in cls.BaseTypes) {
					System.Console.WriteLine("tn:" + typeName + " bt:" + bt.FullName);
					if (bt.FullName == typeName)
						return true;
					
					IType baseCls = ctx.GetType (bt);
					if (baseCls != null && IsValidClass (ctx, baseCls, obj))
						return true;
				}
			}
			return false;
		}
		
		internal static string GetClassName (Stetic.Component obj)
		{
			return GetObjectName (obj);
		}
		
		internal static string GetMemberName (Stetic.Component obj)
		{
			return obj.Name;
		}
		
		internal static string GetObjectName (Stetic.Component obj)
		{
			return obj.Name;
		}
		
		internal static string GetClassName (Stetic.ProjectItemInfo obj)
		{
			return GetObjectName (obj);
		}
		
		internal static string GetObjectName (Stetic.ProjectItemInfo obj)
		{
			return obj.Name;
		}
	}
}
