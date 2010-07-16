using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil.Cil;
using Mono.Cecil;
using System.IO;

namespace Mono.CodeContracts.CCRewrite {
	public static class Rewriter {

		public static RewriterResults Rewrite (RewriterOptions options)
		{
			if (!options.Rewrite) {
				return RewriterResults.Warning("Not asked to rewrite");
			}

			string intputFilename = options.Assembly;
			if (intputFilename == null) {
				return RewriterResults.Error ("No assembly given to rewrite");
			}

			string outputFilename = options.OutputFile ?? intputFilename;

			List<string> errors = new List<string> ();
			List<string> warnings = new List<string> ();

			var assembly = AssemblyDefinition.ReadAssembly (intputFilename);

			bool usingMdb = false;
			bool usingPdb = false;
			if (options.Debug) {
				try {
					ISymbolReaderProvider symProv = new Mono.Cecil.Mdb.MdbReaderProvider ();
					foreach (var module in assembly.Modules) {
						using (ISymbolReader sym = symProv.GetSymbolReader (module, intputFilename)) {
							module.ReadSymbols (sym);
						}
					}
					usingMdb = true;
				} catch {
					try {
						ISymbolReaderProvider symProv = new Mono.Cecil.Pdb.PdbReaderProvider ();
						foreach (var module in assembly.Modules) {
							using (ISymbolReader sym = symProv.GetSymbolReader (module, intputFilename)) {
								module.ReadSymbols (sym);
							}
						}
						usingPdb = true;
					} catch {
					}
				}
			}
			ISymbolWriter symWriter = null;
			if (options.WritePdbFile) {
				if (!options.Debug) {
					return RewriterResults.Error ("Must specify -debug if using -writePDBFile");
				}
				// TODO: Implement symbol writing
				ISymbolWriterProvider symProv = null;
				if (usingMdb) {
					symProv = new Mono.Cecil.Mdb.MdbWriterProvider ();
				} else if (usingPdb) {
					symProv = new Mono.Cecil.Pdb.PdbWriterProvider ();
				} else {
					Console.WriteLine ("No symbol file, cannot write symbols");
				}
				if (symProv != null) {
					symWriter = symProv.GetSymbolWriter (assembly.MainModule, outputFilename);
				}
			}

			PerformRewrite rewriter = new PerformRewrite (symWriter, options);
			rewriter.Rewrite (assembly);

			assembly.Name.Name = Path.GetFileNameWithoutExtension (outputFilename);
			assembly.Write (outputFilename);

			if (symWriter != null) {
				symWriter.Dispose ();
			}

			return new RewriterResults (warnings, errors);
		}

	}
}
