﻿using MongoDB.Driver;
using MongoDB.Bson;
using System.Diagnostics;
using McMaster.Extensions.CommandLineUtils;

public class Example
{
    public static void Main(string[] args) => CommandLineApplication.Execute<Example>(args);

    [Option(Description = "MongoDB connection string")]
    public string? ConnectionString { get; }

    [Option(Description = "Enable strict stable API mode")]
    public bool Strict { get; } = false;

    private void OnExecute()
    {
        var settings = MongoClientSettings.FromConnectionString(ConnectionString);
        if (Strict)
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
