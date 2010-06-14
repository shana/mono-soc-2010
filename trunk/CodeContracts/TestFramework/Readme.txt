This is the test framework for ccrewrite.exe and cccheck.exe

It allows multiple test case programs to be run against the MicroSoft tools and the
results stored.
Then the tools that I am developing can be run and the results checked against the
MicroSoft results.

For this to work the MicroSoft tools need to be placed in the MS_Code directory as follows:

	In MS_Code\CCRewrite
		ccrewrite.exe
	
	In MS_Code\CCCheck
		cccheck.exe
		AbstractDomains.dll
		Analyzers.dll
		CodeAnalysis.dll
		CodeFixes.dll
		ControlFlow.dll
		DataStructures.dll
		Graphs.dll
		
These are not in SVN for obvious reasons, but they can be downloaded from:
http://research.microsoft.com/en-us/projects/contracts/
