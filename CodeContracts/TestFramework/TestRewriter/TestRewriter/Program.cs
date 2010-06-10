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

        private const string RewrittenPathMS = @"..\..\..\__Rewritten\MS";
        private const string RewrittenPathMono = @"..\..\..\__Rewritten\Mono";

        private const string MSRewriterPath = @"..\..\..\..\MS_Code\CCRewrite\ccrewrite.exe";
        private const string MSGACPath = @"C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319";

        static string RewriteMS (string name)
        {
            string testPath = Path.Combine (TestCasesPath, name, TestSubPath, name + ".exe");
            testPath = Path.GetFullPath (testPath);
            string rewrittenPath = Path.Combine (RewrittenPathMS, name + ".exe");
            rewrittenPath = Path.GetFullPath (rewrittenPath);
            Directory.CreateDirectory (Path.GetDirectoryName (rewrittenPath));

            // Use MS ccrewrite.exe
            Process rewriter = new Process ();
            rewriter.StartInfo = new ProcessStartInfo {
                FileName = Path.GetFullPath (MSRewriterPath),
                Arguments = "-assembly " + testPath + " -libpaths " + MSGACPath + " -output " + rewrittenPath + " -throwOnFailure",
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            rewriter.Start ();
            rewriter.WaitForExit ();

            // Copy all DLLs
            var dlls = Directory.EnumerateFiles (Path.GetDirectoryName (testPath), "*.dll");
            foreach (var dll in dlls) {
                string destPath = Path.Combine (Path.GetDirectoryName (rewrittenPath), Path.GetFileName (dll));
                File.Delete (destPath);
                File.Copy (dll, destPath);
            }

            return rewrittenPath;
        }

        static string RunTest (string name)
        {
            string testPath = RewriteMS (name);

            Process test = new Process ();
            test.StartInfo = new ProcessStartInfo {
                FileName = testPath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                ErrorDialog = false
            };
            StringBuilder testOutput = new StringBuilder ();
            test.OutputDataReceived += (sender, e) => {
                testOutput.AppendLine (e.Data);
            };
            test.Start ();
            test.BeginOutputReadLine ();
            test.WaitForExit ();

            return testOutput.ToString ();
        }

        static void StoreMSTestResult (string name)
        {
            string output = RunTest (name);
            string outputPath = Path.Combine (RewrittenPathMS, "_Result_" + name + ".txt");
            outputPath = Path.GetFullPath (outputPath);
            File.WriteAllText (outputPath, output);
            Console.WriteLine ("Test: " + name);
        }

        static void RunMSTests ()
        {
            foreach (var test in Tests) {
                StoreMSTestResult (test);
            }
        }

        static void ShowUsage ()
        {
            Console.WriteLine ("Usage:");
            Console.WriteLine ("  TestRewriter -ms   : Run tests against MS ccrewrite.exe and store results");
            Console.WriteLine ("  TestRewriter -mono : Run tests against Mono ccrewrite.exe and check results");
            Console.WriteLine ();
        }

        static string [] Tests = new [] {
            "Trivial_RequiresEqualsDoubleConst_OK",
            "Trivial_RequiresEqualsDoubleConst_Fail",
            "Trivial_RequiresEqualsIntConst_OK",
            "Trivial_RequiresEqualsIntConst_Fail",
        };

        static void Main (string [] args)
        {
            if (args.Length != 1) {
                ShowUsage();
                return;
            }
            switch (args [0]) {
            case "-ms":
                RunMSTests ();
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
