using MongoDB.Driver;
using MongoDB.Bson;

public static class Example
{
    public static void Main(string[] args)
    {
        var settings = MongoClientSettings.FromConnectionString(args[0].ToString());
        // https://jira.mongodb.org/browse/CSHARP-3516
        // Set the ServerApi field of the settings object to Stable API version 1
        // to force OP_MSG to be used in hello command during handshake.
        settings.ServerApi = new ServerApi(ServerApiVersion.V1);
        var client = new MongoClient(settings);

        IMongoDatabase db = client.GetDatabase("test");
        var command = new BsonDocument { { "ping", 1 } };
        var res = db.RunCommand<BsonDocument>(command);

        Console.WriteLine(res);

        // prevents https://jira.mongodb.org/browse/CSHARP-3429
        client.Cluster.Dispose();
    }
}
