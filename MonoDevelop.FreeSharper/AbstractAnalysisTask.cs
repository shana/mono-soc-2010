// 
// AnalysisTask.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Reflection;

using MonoDevelop.Ide.Gui;

using MonoDevelop.SourceEditor;
using MonoDevelop.CSharp.Parser;
using MonoDevelop.CSharp.Dom;

namespace MonoDevelop.FreeSharper
{
	public class AbstractAnalysisTask : IComparable
	{
		private static CSharpParser parser = new CSharpParser();
		
		public Document TaskDocument {
			get; private set;
		}
		
		public List<RuleExtension> ValidRules {
			get; private set;
		}
		
		public Priorities TaskPriority {
			get; internal set;
		}
		
		public DateTime TaskTimeStamp {
			get; private set;
		}
		
		public DateTime TaskSuspendTime {
			get; private set;
		}
		
		private ManualResetEvent taskSuspendEvent = new ManualResetEvent(false);
		private ManualResetEvent taskTerminateEvent = new ManualResetEvent(false);
		
		private long suspended;
		
		private Thread taskThread;
		private ThreadState failsafeThreadState = ThreadState.Unstarted;
		
		private void ThreadEntry()
		{
			failsafeThreadState = ThreadState.Stopped;
			this.ExecuteTask();
		}
		
		protected bool SuspendIfNeeded()
		{
			bool suspendEventChanged = taskSuspendEvent.WaitOne(0, true);
			if(suspendEventChanged) {
				bool needToSuspend = false;
				if(Interlocked.Read(ref suspended) != 0)
					needToSuspend = true;
				taskSuspendEvent.Reset();
				if(needToSuspend) {
					if(WaitHandle.WaitAny(new WaitHandle[] {taskSuspendEvent, taskTerminateEvent}) > 0) {
						return true;
					}
				}
			}
			
			return false;
		}
		
		protected bool HasTerminatedRequest() 
		{
			return taskTerminateEvent.WaitOne(0, true);
		}
		
		public void Start()
		{
			this.taskThread = new Thread(new ThreadStart(ThreadEntry));
			taskThread.IsBackground = true;
			taskThread.Start();
		}
		
		public void Join()
		{
			if(taskThread != null)
				taskThread.Join();
		}
		
		public bool Join(int millisec)
		{
			if(this.taskThread!=null)
				return this.taskThread.Join(millisec);
			return true;
		}
		
		public bool Join(TimeSpan timeSpan)
		{
			if(this.taskThread!=null)
				return this.taskThread.Join(timeSpan);
			return true;
		}
		
		public void Terminate()
		{
			taskTerminateEvent.Set();
		}
		
		public void TerminateAndWait()
		{
			taskTerminateEvent.Set();
			this.taskThread.Join();
		}
		
		public void Suspend()
		{
			TaskSuspendTime = DateTime.Now;
			while(Interlocked.Exchange(ref suspended, 1) != 1) {}
			taskSuspendEvent.Set();
		}
		
		public void Resume()
		{
			while(Interlocked.Exchange(ref suspended, 0) != 0) {}
			taskSuspendEvent.Set();
		}
		
		public ThreadState TaskState {
			get {
				if(!this.taskThread.Equals(null))
					return this.taskThread.ThreadState;
				return failsafeThreadState;
			}
		}

		public bool IsSuspended {
			get {
				if(this.suspended.Equals(1))
					return true;
				return false;
			}
		}
		
		public bool IsComplete {
			get; private set;
		}
		
		void ValidateRules ()
		{
			foreach (var rule in AnalysisEngineService.ValidRules(TaskDocument))
				ValidRules.Add(rule);
		}
		
		public AbstractAnalysisTask (Document d)
		{
			TaskDocument = d;
			ValidRules = new List<RuleExtension>();
			ValidateRules ();
			TaskTimeStamp = DateTime.UtcNow;
		}
		
		public void ExecuteTask ()
		{
			try {
				CompilationUnit ASTRoot = parser.Parse(TaskDocument.TextEditorData);
				TaskAstVisitor.VisitAstNodes(this, ASTRoot);
			} catch (Exception e) {
				//Log error
			} finally {
				AnalysisExtension.engineCtx.TaskCompletionCallback();
			}
		}
		
		//TODO
		public void ExecuteRules (object node) 
		{
			Type ruleType;
			Type[] paramType;
			object ruleObj;
			foreach(var rule in ValidRules) {
				paramType = new Type[1];
				ruleType = rule.GetType();
				try {
					ruleObj = Activator.CreateInstance(ruleType);
					paramType[0] = node.GetType();
					MethodInfo runRuleMethodInfo = ruleType.GetMethod("RunRule");
					Object[] param = new Object[1];
					param[0] = node;
				} catch (Exception e) {
					//Log error
				}
			}
		}
		
		public int CompareTo (object obj)
		{
			if(obj.Equals(null) || obj.Equals(this))
				return 0;
			
			if(!(obj is AbstractAnalysisTask))
				throw new InvalidCastException();
			
			AbstractAnalysisTask comparerTask = obj as AbstractAnalysisTask;
			if(!comparerTask.TaskSuspendTime.Equals(null) && !this.TaskSuspendTime.Equals(null))
				return comparerTask.TaskTimeStamp.CompareTo(this.TaskTimeStamp);
			return comparerTask.TaskSuspendTime.CompareTo(this.TaskSuspendTime);
		}
		
		public override string ToString ()
		{
			return string.Format("[AbstractAnalysisTask: TaskDocument={0}, ValidRules={1}, TaskPriority={2}, TaskTimeStamp={3}]", TaskDocument, ValidRules, TaskPriority, TaskTimeStamp);
		}
		
	}
}