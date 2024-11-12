using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDbCRUD._1_Domain.Entities;
using MongoDbCRUD._4_Persistence.MogoDb;
using Tests.Modules.SeedDatas;
using Xunit;

namespace Tests.Modules.Fixtures;

public class DatabaseFixture
{
    public const string ConnectionString = "mongodb://root:pass12345@localhost:27017";
    public const string DbName = "CarsMarketDb_Test";
    
    public readonly IMongoCollection<Car> Cars;
    public readonly IMongoCollection<CarSmall> CarsSmall;// Car после редактирования (убрали несколько свойств)
    public readonly IMongoCollection<CarLarge> CarsLarge;// Car после редактирования (добавили несколько свойств)
    
    public readonly IMongoCollection<EntityIntId> EntityIntIds;
    public readonly IMongoCollection<EntityStringId> EntityStringIds;
    public readonly ConnectionThrottlingPipeline ConnectionThrottlingPipeline;

    public DatabaseFixture()
    {
        RegisterClassMap();
        var client= new MongoClient(ConnectionString);
        client.DropDatabase(DbName);
        var database = client.GetDatabase(DbName);
        
        Cars = database.GetCollection<Car>(CarRepository.CollectionName);
        CarsSmall = database.GetCollection<CarSmall>(CarRepository.CollectionName);
        CarsLarge = database.GetCollection<CarLarge>(CarRepository.CollectionName);
        
        EntityIntIds = database.GetCollection<EntityIntId>(EntityIntIdRepository.CollectionName);
        EntityStringIds = database.GetCollection<EntityStringId>(EntityStringIdRepository.CollectionName);
        
        ConnectionThrottlingPipeline = new ConnectionThrottlingPipeline(client.Settings.MaxConnectionPoolSize);
        //Cleanup();
    }


    public void RegisterClassMap()
    {
        BsonClassMap.RegisterClassMap<CarSmall>(cm =>
        {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);
            //cm.UnmapMember(m => m.IsOwner);
        });
        
        BsonClassMap.RegisterClassMap<EntityStringId>(cm =>
        {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);
            cm.MapIdMember(c => c.Key);
            
            //cm.SetDiscriminator("myclass"); //See the polymorphism section for documentation on discriminators and polymorphism. 
            //cm.UnmapMember(m => m.IsOwner);
        });
    }
    
    
    public void Cleanup()
    {
        //Remove all datas
        Cars.DeleteMany(_ => true);
        EntityIntIds.DeleteMany(_ => true);
        EntityStringIds.DeleteMany(_ => true);
        
        //Seed
        Cars.InsertMany(CarsFactory.SeedList);
        EntityIntIds.InsertMany(EntityIntIdsFactory.SeedList);
        EntityStringIds.InsertMany(EntityStringIdsFactory.SeedList);
    }
}


[CollectionDefinition("SharedDbCollection")]
public class SharedDbCollection : ICollectionFixture<DatabaseFixture>
{
}