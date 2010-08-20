BUILDING

You need:
- MonoDevelop assemblies (including dependencies)
- Java installed and ANTLR in class path
- ANTLR C# runtime

RUNNING & DEBUGGING

As binding is a class library, you need to set path to MonoDevelop on Debug tab of FSharpBinding project
Default build assumes that this project folder lies in the same folder MonoDevelop trunk folder lies

DEBUGGING GRAMMAR USING ANTLRworks

As ANTLRworks only works with Java I recommend to comment out language=CSharp2 line in grammar file header.
In this case you'll be able to compile and debug grammar using ANTLRworks only.
Using ANTLRworks you can pass arbitrary input text to parser and view its AST output, parser tree, and debug
parsing process e.g. grammar itself.
