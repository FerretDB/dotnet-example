using MongoDB.Driver;
using MongoDB.Bson;
using System.Diagnostics;
using Microsoft.Extensions.CommandLineUtils;

public class Example
{
    public static void Main(string[] args)
    {
        var commandLineApplication = new CommandLineApplication();
        commandLineApplication.Name = ".NET Example";
        commandLineApplication.Description = "A simple example of using MongoDB with .NET Core";

        var connectionString = commandLineApplication.Argument("connectionString", "MongoDB connection string");
        var strict = commandLineApplication.Option("-s | --strict", "Use strict stable API mode", CommandOptionType.NoValue);

        commandLineApplication.OnExecute(() =>
        {
            Console.WriteLine("connectionString: {0}", connectionString.Value ?? "null");
            Console.WriteLine("strict: {0}", strict.HasValue() ? "true" : "false");

            return 0;
        });

        commandLineApplication.Execute(args);
    }
    
}
