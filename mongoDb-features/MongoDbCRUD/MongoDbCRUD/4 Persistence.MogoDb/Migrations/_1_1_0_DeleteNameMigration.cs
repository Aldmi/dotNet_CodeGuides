using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBMigrations;
using Version = MongoDBMigrations.Version;


namespace Tests.Modules.Migration.MogoDb.Tests.Migrations;

public class _1_1_0_DeleteNameMigration : IMigration
{
    public void Up(IMongoDatabase database)
    {
        // var collection = database.GetCollection<BsonDocument>("Cars");
        // collection.UpdateMany(
        //     FilterDefinition<BsonDocument>.Empty,
        //     Builders<BsonDocument>.Update.Rename("Name", "firstName_1"));
    }

    public void Down(IMongoDatabase database)
    {
        var collection = database.GetCollection<BsonDocument>("Cars");
        collection.UpdateMany(
            FilterDefinition<BsonDocument>.Empty,
            Builders<BsonDocument>.Update.Rename("firstName_1", "Name"));
    }

    public Version Version => new Version(1, 1, 0);
    public  string  Name => "Delete Name field";
}