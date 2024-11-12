using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBMigrations;

namespace Tests.Modules.Migration.MogoDb.Tests.Migrations;

public class _1_0_0_FirstMigration: IMigration
{
    public void Up(IMongoDatabase database)
    {
        var collection = database.GetCollection<BsonDocument>("Cars");
        collection.UpdateMany(
            FilterDefinition<BsonDocument>.Empty,
            Builders<BsonDocument>.Update.Rename("Name", "firstName_1"));
    }

    public void Down(IMongoDatabase database)
    {
       
    }

    public Version Version => new MongoDBMigrations.Version(1, 0, 0);
    public  string  Name => "Name -> firstName_1";
}