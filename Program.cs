using MongoDB.Driver;
using MongoDB.Bson;

const string connectionUri = "mongodb://localhost:27017/?directConnection=true";

var settings = MongoClientSettings.FromConnectionString(connectionUri);

var client = new MongoClient(connectionUri);

var collection = client.GetDatabase("test").GetCollection<BsonDocument>("foo");

var filter = Builders<BsonDocument>.Filter.Eq("name", "foo");

var res = collection.Find(filter).First();

Console.WriteLine(res);
