Implementation of System.Diagnostics.Contracts namespace.
Currently in a stand-alone library that can be compiled and tested against .NET 3.5

Tests marked in category RunAgainstReference can be run against MS.NET 4.0 successfully.
Tests that trigger assertions cannot be run against MS.NET 4.0 because it is not possible
to intercept these assertions.

Implementation is complete.
All tests pass.
All classes, methods have documentation, although this may need expanding later.


CodeContracts.patch can be applied to corlib and fully implements the
System.Diagnostics.Contracts namespace and adds a set of tests.
