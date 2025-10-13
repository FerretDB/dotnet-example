using MongoDB.Driver;
using MongoDB.Bson;
using System.Diagnostics;
using System.CommandLine;

public static class Example
{
    public static int Main(string[] args)
    {
        var connectionString = new Argument<string>("connectionString");
        connectionString.Description = "MongoDB connection string";

        var strict = new Option<bool?>("--strict", new[] { "-s" });
        strict.Description = "Use strict stable API mode.";
        strict.DefaultValueFactory = _ => false;

        var rootCommand = new RootCommand("A simple example of using MongoDB with .NET Core");
        rootCommand.Add(connectionString);
        rootCommand.Add(strict);

        rootCommand.SetAction((parseResult) => 
        {
            Handle(parseResult.GetRequiredValue(connectionString), parseResult.GetValue(strict));
        });

        var parseResult = rootCommand.Parse(args);
        return parseResult.Invoke(new InvocationConfiguration());
    }

    private static void Handle(string connectionString, bool? strict)
    {
        var settings = MongoClientSettings.FromConnectionString(connectionString);

        if (Convert.ToBoolean(strict))
        {
            var serverApi = new ServerApi(ServerApiVersion.V1, strict: true);
            settings.ServerApi = serverApi;
        }

        var client = new MongoClient(settings);

        IMongoDatabase db = client.GetDatabase("test");
        var command = new BsonDocument { { "ping", 1 } };
        var res = db.RunCommand<BsonDocument>(command);
        Debug.Assert(res["ok"].ToDouble() == 1.0, "ping failed");

        command = new BsonDocument { { "dropDatabase", 1 } };
        res = db.RunCommand<BsonDocument>(command);
        Debug.Assert(res["ok"].ToDouble() == 1.0, "dropDatabase failed");

        var documentList = new List<BsonDocument>{
            new BsonDocument { { "_id", 1 }, { "a", 1 } },
            new BsonDocument { { "_id", 2 }, { "a", 2 } },
            new BsonDocument { { "_id", 3 }, { "a", 3 } },
            new BsonDocument { { "_id", 4 }, { "a", 4 } },
        };

        var collection = db.GetCollection<BsonDocument>("foo");
        collection.InsertMany(documentList);

        var filter = Builders<BsonDocument>.Filter.Eq("a", 4);
        BsonDocument actual = collection.Find(filter).FirstOrDefault();
        Debug.Assert(actual == new BsonDocument { { "_id", 4 }, { "a", 4 } }, "Value should be 4");

        // prevents https://jira.mongodb.org/browse/CSHARP-3429
        client.Cluster.Dispose();
    }

}
