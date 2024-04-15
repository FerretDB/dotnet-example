# FerretDB .NET example

This is a fork of https://github.com/mongodb-developer/mongodb-dotnet-example,
reduced to reproduce a (already fixed) problem with FerretDB.

How to run:

1. Install `dotnet` tool.
-  Run `dotnet run --connection-string=mongodb://localhost:27017/`.
- To enble the strict Stable API use the `--strict` flag.
- Run with PLAIN authentication `dotnet run "mongodb://username:password@localhost:27017/?authMechanism=PLAIN"`.
