#define CONTRACTS_FULL

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.IO;
using Mono.CodeContracts.CCRewrite;
using Mono.Options;

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

			if (args.Length > 0 && args [0] == "test") {
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

			RewriterOptions options = new RewriterOptions ();

			bool showOptions = false;
			string showMsg = null;

			var optionSet = new OptionSet {
				{ "help", "Show this help.", v => showOptions = v != null },
				{ "debug", "Use MDB or PDB debug information (default=true).", v => options.Debug = v != null },
				{ "level=", "Instrumentation level, 0 - 4 (default=4).", (int var) => options.Level = var},
				{ "writePDBFile", "Write MDB or PDB file (default=true).", v => options.WritePdbFile = v != null },
				{ "rewrite", "Rewrite the assembly (default=true).", v => options.Rewrite = v != null },
				{ "assembly=", "Assembly to rewrite.", v => options.Assembly = v },
				{ "breakIntoDebugger|break", "Break into debugger on contract failure.", v => options.BreakIntoDebugger = v != null },
				{ "throwOnFailure|throw", "Throw ContractException on contract failure.", v => options.ThrowOnFailure = v != null },
				{ "output|out=", "Output filename of rewritten file.", v => options.OutputFile = v },
			};

			try {
				optionSet.Parse (args);
			} catch (OptionException e) {
				showOptions = true;
				showMsg = e.Message;
			}

			if (showOptions) {
				Console.WriteLine ("ccrewrite");
				Console.WriteLine ();
				Console.WriteLine ("Options:");
				optionSet.WriteOptionDescriptions (Console.Out);
				Console.WriteLine ();
				if (showMsg != null) {
					Console.WriteLine (showMsg);
					Console.WriteLine ();
				}
				return;
			}

			var results = Rewriter.Rewrite (options);
			if (results.AnyErrors) {
				foreach (var error in results.Errors) {
					Console.WriteLine ("Error: " + error);
				}
			}
			if (results.AnyWarnings) {
				foreach (var warning in results.Warnings) {
					Console.WriteLine ("Warning: " + warning);
				}
			}

			Console.WriteLine ();
			Console.WriteLine ("*** done ***");
			//Console.ReadKey ();
		}

	}
}
