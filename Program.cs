using MongoDB.Driver;
using MongoDB.Bson;

public static class Example
{
    public static void Main(string[] args)
    {
        var client = new MongoClient(args[0].ToString());

        IMongoDatabase db = client.GetDatabase("test");
        var command = new BsonDocument { { "ping", 1 } };
        var res = db.RunCommand<BsonDocument>(command);

        Console.WriteLine(res);

        // prevents https://jira.mongodb.org/browse/CSHARP-3429
        client.Cluster.Dispose();
    }
}
