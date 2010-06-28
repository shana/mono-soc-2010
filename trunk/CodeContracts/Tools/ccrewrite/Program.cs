#define CONTRACTS_FULL

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Decompiler;
using Decompiler.Ast;
using Decompiler.Visitors;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace ccrewrite {

	class A {
		public virtual void Test (int i)
		{
			Contract.Requires (i == 4);
		}
	}

	class B : A {
		public override void Test (int i)
		{
		}
	}

    class Program {

        static void Test1 (sbyte a, short b, int c, long d, byte e, ushort f, uint g, ulong h, float i, double j, object o)
        {
			Contract.Requires (c + 100 == 101);
			Contract.Requires (c + 1000 == 1001);
			Contract.Requires (a + b + c == 3);
			Contract.Requires (c - b == 0);

			Contract.Requires (a == 1);
			Contract.Requires (b == 1);
			Contract.Requires (c == 1);
			Contract.Requires (d == 1);
			Contract.Requires (e == 1);
			Contract.Requires (f == 1);
			Contract.Requires (g == 1);
			Contract.Requires (h == 1);
			Contract.Requires (i == 1);
			Contract.Requires (j == 1);

			Contract.Requires (a >= 1);
			Contract.Requires (b >= 1);
			Contract.Requires (c >= 1);
			Contract.Requires (d >= 1);
			Contract.Requires (e >= 1);
			Contract.Requires (f >= 1);
			Contract.Requires (g >= 1);
			Contract.Requires (h >= 1);
			Contract.Requires (i >= 1);
			Contract.Requires (j >= 1);

			Contract.Requires (a <= 1);
			Contract.Requires (b <= 1);
			Contract.Requires (c <= 1);
			Contract.Requires (d <= 1);
			Contract.Requires (e <= 1);
			Contract.Requires (f <= 1);
			Contract.Requires (g <= 1);
			Contract.Requires (h <= 1);
			Contract.Requires (i <= 1);
			Contract.Requires (j <= 1);

			Contract.Requires (a > 0);
			Contract.Requires (b > 0);
			Contract.Requires (c > 0);
			Contract.Requires (d > 0);
			Contract.Requires (e > 0);
			Contract.Requires (f > 0);
			Contract.Requires (g > 0);
			Contract.Requires (h > 0);
			Contract.Requires (i > 0);
			Contract.Requires (j > 0);

			Contract.Requires (a < 2);
			Contract.Requires (b < 2);
			Contract.Requires (c < 2);
			Contract.Requires (d < 2);
			Contract.Requires (e < 2);
			Contract.Requires (f < 2);
			Contract.Requires (g < 2);
			Contract.Requires (h < 2);
			Contract.Requires (i < 2);
			Contract.Requires (j < 2);

			Contract.Requires (o != null);
			Contract.Requires (o != null, "Mymsg");

			Console.Write ("All contracts written: {0}", a * b);
        }

        static void Main (string [] args)
        {

			if (args.Length > 0 && args[0] == "test") {
				//Test1 (1, 1, 1, 1, 1, 1, 1, 1, 1, 1, new object ());
				B b = new B ();
				try {
					b.Test (0);
					Console.WriteLine ("No exception");
				} catch (Exception e) {
					Console.WriteLine (e);
				}
				return;
			}

			bool ok = CmdOptions.Initialise (args);
			if (!ok) {
				return;
			}

			if (CmdOptions.Assembly == null) {
				CmdOptions.ShowUsage ("Error: No assembly given to rewrite");
				return;
			}

			string filename = CmdOptions.Assembly;

			var assembly = AssemblyDefinition.ReadAssembly (filename);
			var mod = assembly.MainModule;

			if (CmdOptions.Debug) {
				try {
					ISymbolReaderProvider symProv = new Mono.Cecil.Mdb.MdbReaderProvider ();
					ISymbolReader sym = symProv.GetSymbolReader (mod, filename);
					mod.ReadSymbols (sym);
				} catch {
					try {
						ISymbolReaderProvider symProv = new Mono.Cecil.Pdb.PdbReaderProvider ();
						ISymbolReader sym = symProv.GetSymbolReader (mod, filename);
						mod.ReadSymbols (sym);
					} catch {
					}
				}
			}
			if (CmdOptions.WritePdbFile) {
				if (!CmdOptions.Debug) {
					CmdOptions.ShowUsage ("Must specify -debug if using -writePDBFile");
					return;
				}
				// TODO: Implement symbol writing
			}

			ContractsRuntime.Initialise (mod);

			if (CmdOptions.Rewrite) {
				var rewriter = new Rewriter ();
				rewriter.Rewrite (assembly);
				assembly.Write (CmdOptions.OutputFile ?? CmdOptions.Assembly);
			}

            Console.WriteLine ();
            Console.WriteLine ("*** done ***");
            Console.ReadKey ();
        }

    }
}
