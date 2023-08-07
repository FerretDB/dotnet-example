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

        Console.WriteLine(res);

        command = new BsonDocument { { "dropDatabase", 1 } };
        res = db.RunCommand<BsonDocument>(command);

        Console.WriteLine(res);

        var documentList = new List<BsonDocument>();
        for (int i = 1; i < 5; i++)
        {
            var document = new BsonDocument{ {"_id", i }, { "a", i } };
            documentList.Add(document);
        }

        // insert documents
        var collection = db.GetCollection<BsonDocument> ("foo");
        collection.InsertMany(documentList);

        // find the document
        var filter = Builders<BsonDocument>.Filter.Eq("a", 4);
        var resList = collection.Find(filter).ToList();
        BsonDocument actual = resList.Last(); 

        var expected = new BsonDocument { { "_id", 4 }, { "a", 4 } };
        Debug.Assert(expected == actual, " Value should be 4");

        // prevents https://jira.mongodb.org/browse/CSHARP-3429
        client.Cluster.Dispose();
    }
}
