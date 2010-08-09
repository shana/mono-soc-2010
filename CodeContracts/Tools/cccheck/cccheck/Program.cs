using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using cccheck.Ast;
using cccheck.Decompiler;

namespace cccheck {
	class Program {

		static Dictionary<MethodDefinition, Expr> Decompiled = new Dictionary<MethodDefinition, Expr> ();

		static void DecompileAll (MethodDefinition entryPoint)
		{
			Stack<MethodDefinition> todo = new Stack<MethodDefinition> ();
			todo.Push (entryPoint);

			while (todo.Count > 0) {
				var method = todo.Pop ();
				if (Decompiled.ContainsKey (method)) {
					continue;
				}

				var vDecompile = new Decompile (method);
				var decompile = vDecompile.Go (false);
				Decompiled [method] = decompile;

				var vFindMethodRefs = new FindMethodCallsVisitor ();
				vFindMethodRefs.Visit (decompile);

				foreach (var call in vFindMethodRefs.Calls) {
					if (call.DeclaringType.FullName == "System.Diagnostics.Contracts.Contract") {
						continue;
					}
					var calledMethod = call.Resolve ();
					if (!Decompiled.ContainsKey (calledMethod)) {
						todo.Push (calledMethod);
					}
				}
			}
		}

		static void Main (string [] args)
		{
			string filename = @"..\..\..\CcCheckTest\bin\debug\CcCheckTest.exe";

			var assembly = AssemblyDefinition.ReadAssembly (filename);
			var module = assembly.MainModule;
			var entryPointMethod = module.EntryPoint;

			DecompileAll (entryPointMethod);

			Environment env = new Environment ();

			var d = Decompiled [entryPointMethod];

			CheckVisitor v = new CheckVisitor (env);
			v.Visit (d);

			foreach (var call in v.Calls) {
				var method = call.Call.Method.Resolve ();
				env = new Environment (call.Call.Method.Parameters, call.Parameters);
				d = Decompiled [method];
				v = new CheckVisitor (env);
				v.Visit (d);
			}

			Console.ReadKey ();
		}
	}
}
