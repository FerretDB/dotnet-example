# FerretDB .NET example

This is a fork of https://github.com/mongodb-developer/mongodb-dotnet-example,
reduced to reproduce a (already fixed) problem with FerretDB.

How to run:

1. Install `dotnet` tool.
- Run `dotnet run mongodb://localhost:27017/`.
- To run with the strict Stable API use the `--strict` flag.
- To run with PLAIN authentication pass PLAIN to the `authMechanism` URI option `dotnet run "mongodb://username:password@localhost:27017/?authMechanism=PLAIN"`.
