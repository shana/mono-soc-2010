// 
// AnalysisEngineService.cs
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
using System.Collections;
using System.Threading;
using System.Linq;

using C5;

using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.FreeSharper
{
	public class AnalysisEngineService : IDisposable
	{
		#region RuleRepository
		
		internal static object syncRoot = new object();
		public static Dictionary<string, RuleExtension> AnalysisRules = new Dictionary<string, RuleExtension>();
		
		public static IEnumerable<RuleExtension> ValidRules(Document d)
		{
			lock(syncRoot)
				foreach(var rule in AnalysisRules) {
					if(rule.Value.IsValidForDocument)
						yield return rule.Value;
				}
		}
		
		public static void RegisterRule(RuleExtension rule)
		{
			if(rule != null) {
				if(!AnalysisRules.ContainsKey(rule.RuleId)) {
					lock(syncRoot)
						AnalysisRules.Add(rule.RuleId, rule);
				}
			}
		}
		
		public static IEnumerable<RuleExtension> GetRegisteredRules()
		{
			lock(syncRoot)
				foreach(var rule in AnalysisRules) {
					yield return rule.Value;
			}
		}
		
		public static void DeregisterRule(RuleExtension rule)
		{
			if(rule!=null) {
				if(AnalysisRules.ContainsKey(rule.RuleId)) {
					lock(syncRoot)
						AnalysisRules.Remove(rule.RuleId);
				}
			}
		}
		
		public static void FlushRules()
		{
			if(AnalysisRules.Count>0){
				lock(syncRoot)
					AnalysisRules.Clear();
			}
		}
		#endregion
		
		#region Jobqueue logic
		private const int maxTask = 15;
		
		C5.IDictionary<string ,IPriorityQueueHandle<AbstractAnalysisTask>> taskDictionary;
  		IntervalHeap<AbstractAnalysisTask> priorityTaskQueue;
		
		AbstractAnalysisTask activeTask;
		
		EventWaitHandle engineWaitHandle = new AutoResetEvent (false);
		
		Thread analysisThread;
		
		bool taskRunning = false;
		bool disposed;
		
		public AnalysisEngineService() 
		{
			disposed = false;
			priorityTaskQueue = new IntervalHeap<AbstractAnalysisTask>();
			taskDictionary = new C5.HashDictionary<string, IPriorityQueueHandle<AbstractAnalysisTask>>();
			analysisThread = new Thread (Work);
   			analysisThread.Start();
		}
		
		public void EnqueueTask (AbstractAnalysisTask d) 
		{
	   		lock (syncRoot) {
				if(this.priorityTaskQueue.Count > maxTask)
					return;
				IPriorityQueueHandle<AbstractAnalysisTask> taskHandle = null;
    			if(!this.priorityTaskQueue.Add(ref taskHandle, d))
					throw new Exception("Could not enqueue task: " + d.ToString());
				taskDictionary.Add(d.ToString(), taskHandle);
    			if(priorityTaskQueue.Count == 1)
					engineWaitHandle.Set();
			}
		}
	
		public AbstractAnalysisTask DequeueTask() 
		{
			lock (syncRoot) {
				if (!priorityTaskQueue.IsEmpty) {
      				AbstractAnalysisTask task = priorityTaskQueue.DeleteMax();
					taskDictionary.Remove(task.ToString());
					return task;
				} else {
					return null;
				}
			}
		}
		
		public void EnqueueTaskWithPriority (AbstractAnalysisTask task)
		{
			lock(syncRoot) {
				if(taskRunning) {
					activeTask.Terminate();
					activeTask = task;
					taskRunning = true;
					activeTask.Start();
				}
			}
		}
		
		public void Flush (Document d)
		{
			lock(syncRoot) {
				foreach(var task in taskDictionary.Where(task => task.Key.Contains(d.FileName))) {
					IPriorityQueueHandle<AbstractAnalysisTask> h;
			    	if (this.taskDictionary.Remove(task.Key, out h)) {
				    	this.priorityTaskQueue.Delete(h);
					}
				}
			}
		}
		
		void Work() 
		{
			AbstractAnalysisTask task = null;
			//TODO As some tasks can take a long time to complete, we need some sort of a scheduling policy so that other tasks can run as well
			while (!disposed) {
				if(taskRunning)
					continue;
				if(priorityTaskQueue.Count > 0) {
					task = this.DequeueTask();
					if (task == null) 
						throw new Exception("Null task encountered");
				    taskRunning = true;
					activeTask = task;
					if(activeTask.IsSuspended)
						activeTask.Resume();
					else
						activeTask.Start();	
				} else {
			   		engineWaitHandle.WaitOne();
				}
			}
		}
		
		public void SuspendActiveTask(object sender, EventArgs args) 
		{
			lock(syncRoot) {
				if(activeTask!=null && !activeTask.IsSuspended) {
					activeTask.Suspend();
					this.EnqueueTask(activeTask);
					activeTask = null;
					taskRunning = false;
				}
			}
		}
		
		public void TaskCompletionCallback()
		{
			lock(syncRoot) {
				taskRunning = false;
			}	
		}
		#endregion
		
		public void Dispose () 
		{
			if(disposed!=true)
				disposed = true;
	   		analysisThread.Abort();
	   		engineWaitHandle.Close();
	  	}
	}
}
