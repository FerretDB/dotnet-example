using MongoDB.Driver;
using MongoDB.Bson;
using System.Diagnostics;

public static class Example
{
    public static void Main(string[] args)
    {
        var connectionUri = args[0].ToString();
        var settings = MongoClientSettings.FromConnectionString(connectionUri);
        var client = new MongoClient(settings);

        IMongoDatabase db = client.GetDatabase("test");
        var command = new BsonDocument { { "ping", 1 } };
        var res = db.RunCommand<BsonDocument>(command);
        Debug.Assert(res == new BsonDocument { { "ok", 1.0 } }, "ping failed");

        command = new BsonDocument { { "dropDatabase", 1 } };
        res = db.RunCommand<BsonDocument>(command);
        Debug.Assert(res == new BsonDocument { { "ok", 1.0 } }, "dropDatabase failed");

        var documentList = new List<BsonDocument>{
            new BsonDocument { { "_id", 1 }, { "a", 1 } },
            new BsonDocument { { "_id", 2 }, { "a", 2 } },
            new BsonDocument { { "_id", 3 }, { "a", 3 } },
            new BsonDocument { { "_id", 4 }, { "a", 4 } },
        };

        var collection = db.GetCollection<BsonDocument>("foo");
        collection.InsertMany(documentList);

        var filter = Builders<BsonDocument>.Filter.Eq("a", 4);
        BsonDocument actual = collection.Find(filter).ToList().Last();
        var expected = new BsonDocument { { "_id", 4 }, { "a", 4 } };
        Debug.Assert(expected == actual, "Value should be 4");

        // prevents https://jira.mongodb.org/browse/CSHARP-3429
        client.Cluster.Dispose();
    }
}
