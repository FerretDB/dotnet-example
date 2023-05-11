// how to run:
// 1. install dotnet runtime https://learn.microsoft.com/en-us/dotnet/core/install/linux-scripted-manual#scripted-install
// 2. export DOTNET_ROOT=$HOME/.dotnet
// 3. export PATH=$PATH:$HOME/.dotnet:$HOME/.dotnet/tools
// 4. dotnet add package MongoDB.Driver, if you cannot add the package you may have to first run `dotnet new console --force`
// 5. dotnet run

using MongoDB.Driver;
using MongoDB.Bson;

const string connectionUri = "mongodb://localhost:27017/?directConnection=true";

var settings = MongoClientSettings.FromConnectionString(connectionUri);

var client = new MongoClient(connectionUri);

var collection = client.GetDatabase("test").GetCollection<BsonDocument>("foo");

var filter = Builders<BsonDocument>.Filter.Eq("name", "foo");

var res = collection.Find(filter).First();

Console.WriteLine(res);
