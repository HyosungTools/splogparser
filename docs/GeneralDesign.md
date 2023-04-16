# General Design

A general summary of the design for those wanted to contribute. I assume you have the splogparser.sln file open in Visual Studio.

The application is one exe (splogparser.exe) and a bunch of DLLs (e.g. CDMView.dll). There is also some xfd and xml files as well. Its a .NET Framework 4.7.2 application.

## splogparser

The splogparser (`Program.ps`) is the main workflow of the application. It does the following: 

1. Gathers information from the command line.
2. Builds the Context.
3. Unzips the argument.
4. Finds all trace files.
5. Finds all Views.
6. For each View it calls Process, WriteExcel and Cleanup.

## The Context

The Context is a list of objects passed around that code can use to do stuff - like talk to the file system or write log lines. The idea is each object in the context is an interface (e.g. `IFileSystemProvider`) so you can build whatever Context you like (mostly I want to build a mock Context so I can extend unit testing - its on my TODO list).

You can put whatever you want in the context, for example - a list of references to all Views found. YOu might want to do this if you wanted to do some post-processing steps. The context makes the application loosely coupled.

## Find All Views

This is neat. The application discovers which View to process at runtime (see `ViewLoader.cs`). You can extend this application by writing your own DLL; as long as it adheres to interface contract the application will find and run it.
