using MongoDB.Driver;
using MongoDB.Bson;

public static class Example
{
    public static void Main(string[] args)
    {
        foreach (Object obj in args) {
            string connectionUri = obj.ToString();
            var client = new MongoClient(connectionUri);

            IMongoDatabase db = client.GetDatabase("test");
            var command = new BsonDocument { { "ping", 1 } };
            var res = db.RunCommand<BsonDocument>(command);

            Console.WriteLine(res);
        }
    }
}
