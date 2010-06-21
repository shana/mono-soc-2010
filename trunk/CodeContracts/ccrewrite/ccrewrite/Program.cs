#define CONTRACTS_FULL

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using System.Diagnostics.Contracts;
using Mono.Cecil.Cil;
using Decompiler;
using Decompiler.Visitors;
using Decompiler.Ast;

namespace ccrewrite {
    class Program {

        static void Test1 (sbyte a, short b, int c, long d, byte e, ushort f, uint g, ulong h, float i, double j)
        {
			Contract.Requires (a == 0);
			Contract.Requires (b == 0);
			Contract.Requires (c == 0);
			Contract.Requires (d == 0);
			Contract.Requires (e == 0);
			Contract.Requires (f == 0);
			Contract.Requires (g == 0);
			Contract.Requires (h == 0);
			Contract.Requires (i == 0);
			Contract.Requires (j == 0);

			Console.Write ("All contracts written");
        }

        static void Rewrite (MethodDefinition method)
        {
            var body = method.Body;
			Decompile decompile = new Decompile (method);
			var decomp = decompile.Go ();

			TransformContractsVisitor vTransform = new TransformContractsVisitor (method);
			var transformed = vTransform.Visit (decomp);

			var il = body.GetILProcessor ();

			foreach (var replacement in vTransform.ToReplace) {
				Expr exprRemove = replacement.Item1;
				Expr exprInsert = replacement.Item2;
				var vInstExtent = new InstructionExtentVisitor (decompile.Instructions);
				vInstExtent.Visit (exprRemove);
				var instBeforeFirst = vInstExtent.Instructions.First ().Previous;
				Action<Instruction> fnEmit;
				if (instBeforeFirst != null) {
					var instInsertAfter = instBeforeFirst;
					fnEmit = inst => {
						il.InsertAfter (instInsertAfter, inst);
						instInsertAfter = inst;
					};
				} else {
					throw new NotImplementedException ();
				}
				foreach (var instRemove in vInstExtent.Instructions) {
					il.Remove (instRemove);
				}
				var compiler = new CompileVisitor (il, fnEmit);
				compiler.Visit (exprInsert);
			}
			
        }

        static void Main (string [] args)
        {

			if (args.Length > 0) {
				Test1 (0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
				return;
			}

            var ass = AssemblyDefinition.ReadAssembly ("ccrewrite.exe");
			var mod = ass.MainModule;
			ContractsRuntime.Initialise (mod);

            var allMethods =
                from module in ass.Modules
                from type in module.Types
                from method in type.Methods
                select method;

            foreach (var m in allMethods.ToArray()) {
                if (m.Name != "Test1") {
                    continue;
                }
                Console.WriteLine (m.FullName);
                Rewrite (m);
            }

			ass.Write ("ccrewrite_2.exe");

            Console.WriteLine ();
            Console.WriteLine ("*** done ***");
            Console.ReadKey ();
        }

    }
}
