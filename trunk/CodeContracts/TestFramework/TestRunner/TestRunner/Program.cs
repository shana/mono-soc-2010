using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace TestRewriter {
    class Program {

        private const string TestCasesPath = @"..\..\..\..\TestCases";
        private const string TestSubPath = @"bin\debug";

        private const string MSGACPath = @"C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319";

        private const string RewriterPath_MS = @"..\..\..\..\MS_Code\CCRewrite\ccrewrite.exe";
        private const string RewrittenPath_MS = @"..\..\..\__Rewritten\MS";
        private const string RewrittenPath_Mono = @"..\..\..\__Rewritten\Mono";

        private const string CcCheckPath_MS = @"..\..\..\..\MS_Code\CCCheck\cccheck.exe";
        private const string CcCheckedPath_MS = @"..\..\..\__CcCheck\MS";

        static void RunTest_CcCheck_MS (string name)
        {
            Process cccheck = new Process ();
            cccheck.StartInfo = new ProcessStartInfo {
                FileName = Path.GetFullPath (CcCheckPath_MS),
                Arguments =
                    @"-includeCompilerGenerated -nobox -nologo -nopex -remote -arithmetic:type=intervals " +
                    @"-libPaths:""C:\Program Files\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\Profile\Client\;" +
                    @"C:\Program Files\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\Profile\Client\CodeContracts"" " +
                    @"-libPaths:""C:\Program Files\Microsoft\Contracts\Contracts\.NETFramework\v4.0"" " +
                    @"""" + name + @"""",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            StringBuilder output = new StringBuilder ();
            cccheck.OutputDataReceived += (sender, e) => {
                output.AppendLine (e.Data);
            };
            StringBuilder error = new StringBuilder ();
            cccheck.ErrorDataReceived += (sender, e) => {
                error.AppendLine (e.Data);
            };
            cccheck.Start ();
            cccheck.BeginOutputReadLine ();
            cccheck.BeginErrorReadLine ();
            cccheck.WaitForExit ();

            string allOutput = output.ToString () + error.ToString ();
            string outputPath = Path.Combine (CcCheckedPath_MS, "_Result_" + Path.GetFileName(name) + ".txt");
            outputPath = Path.GetFullPath (outputPath);
            Directory.CreateDirectory (Path.GetDirectoryName (outputPath));
            File.WriteAllText (outputPath, allOutput);
        }

        static string Rewrite_MS (string name)
        {
            string rewrittenPath = Path.Combine (RewrittenPath_MS, Path.GetFileName(name));
            rewrittenPath = Path.GetFullPath (rewrittenPath);
            Directory.CreateDirectory (Path.GetDirectoryName (rewrittenPath));

            // Use MS ccrewrite.exe
            Process rewriter = new Process ();
            rewriter.StartInfo = new ProcessStartInfo {
                FileName = Path.GetFullPath (RewriterPath_MS),
                Arguments = "-assembly \"" + name + "\" -libpaths \"" + MSGACPath + "\" -output \"" + rewrittenPath + "\" -throwOnFailure",
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            rewriter.Start ();
            rewriter.WaitForExit ();

            // Copy all DLLs
            var dlls = Directory.EnumerateFiles (Path.GetDirectoryName (name), "*.dll");
            foreach (var dll in dlls) {
                string destPath = Path.Combine (Path.GetDirectoryName (rewrittenPath), Path.GetFileName (dll));
                File.Delete (destPath);
                File.Copy (dll, destPath);
            }

            return rewrittenPath;
        }

        static void RunTest_Rewrite_MS (string name)
        {
            string testPath = Rewrite_MS (name);

            Process testProcess = new Process ();
            testProcess.StartInfo = new ProcessStartInfo {
                FileName = testPath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                ErrorDialog = false
            };
            StringBuilder testOutput = new StringBuilder ();
            testProcess.OutputDataReceived += (sender, e) => {
                testOutput.AppendLine (e.Data);
            };
            testProcess.Start ();
            testProcess.BeginOutputReadLine ();
            testProcess.WaitForExit ();

            string output = testOutput.ToString ();
            string outputPath = Path.Combine (RewrittenPath_MS, "_Result_" + Path.GetFileName(name) + ".txt");
            outputPath = Path.GetFullPath (outputPath);
            Directory.CreateDirectory (Path.GetDirectoryName (outputPath));
            File.WriteAllText (outputPath, output);
        }

        static void RunAllTests_MS ()
        {
            var tests =
                from exe in Directory.EnumerateFiles (TestCasesPath, "*.exe", SearchOption.AllDirectories)
                let dir = Path.GetDirectoryName (exe)
                let filename = Path.GetFileName(exe)
                where dir.EndsWith ("bin" + Path.DirectorySeparatorChar + "Debug")
                where !filename.EndsWith(".vshost.exe")
                select Path.GetFullPath(exe);

            foreach (var test in tests) {
                Console.WriteLine ("Test: " + Path.GetFileName (test));

                Console.Write("  Rewrite ...");
                RunTest_Rewrite_MS (test);
                Console.WriteLine (" done");

                Console.Write("  CcCheck ...");
                RunTest_CcCheck_MS (test);
                Console.WriteLine (" done");
            }
        }

        static void ShowUsage ()
        {
            Console.WriteLine ("Usage:");
            Console.WriteLine ("  TestRewriter -ms   : Run tests against MS ccrewrite.exe and store results");
            Console.WriteLine ("  TestRewriter -mono : Run tests against Mono ccrewrite.exe and check results");
            Console.WriteLine ();
        }

        //static string [] Tests = new [] {
        //    "Trivial_RequiresEqualsDoubleConst_OK",
        //    "Trivial_RequiresEqualsDoubleConst_Fail",
        //    "Trivial_RequiresEqualsIntConst_OK",
        //    "Trivial_RequiresEqualsIntConst_Fail",
        //};

        static void Main (string [] args)
        {
            if (args.Length != 1) {
                ShowUsage();
                return;
            }
            switch (args [0]) {
            case "-ms":
                RunAllTests_MS ();
                break;
            case "-mono":
                throw new NotImplementedException ();
            default:
                ShowUsage ();
                return;
            }

            Console.WriteLine ();
            Console.WriteLine ("Done");
        }
    }
}
