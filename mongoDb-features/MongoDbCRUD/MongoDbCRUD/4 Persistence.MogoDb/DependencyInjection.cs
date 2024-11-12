using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDbCRUD._1_Domain.Entities;
using MongoDbCRUD._2_Persistence.Interfaces;
using MongoDbCRUD._3_Application;

namespace MongoDbCRUD._4_Persistence.MogoDb;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var maxConnectionPoolSize = 800;
        services.AddSingleton<IMongoDatabase>(_ =>
        {
            var connectionString= configuration.GetConnectionString("Mongodb");
            var url = MongoUrl.Create(connectionString);
            var client = new MongoClient(new MongoClientSettings()
            {
                Server = url.Server,
                Credential = url.GetCredential(), //user, password
                MinConnectionPoolSize = 100,
                MaxConnectionPoolSize = maxConnectionPoolSize,
                // WaitQueueTimeout = TimeSpan.FromSeconds(100),
                // ConnectTimeout = TimeSpan.FromSeconds(10),
                ConnectTimeout = TimeSpan.FromSeconds(10)
            });
            var dbName = "CarsMarket_Prod";
            return client.GetDatabase(dbName);
        });
        
        //реализация интерфейса IMongoCollection по умолчанию потокобезопасна
        services.AddSingleton<IMongoCollection<Car>>(provider =>
        {
            var db= provider.GetService<IMongoDatabase>();
            return db.GetCollection<Car>(CarRepository.CollectionName);
        });
        
        services.AddSingleton(_ => new ConnectionThrottlingPipeline(maxConnectionPoolSize) );// Общий экземпляр для нескольких репозиториев (кому он нужен.)
        services.AddSingleton<ICarRepository, CarRepository>();
        
        
        services.AddSingleton<IDbMigrationService, MongoDbMigrationService>();
        
        services.AddHostedService<SeedDbHostedService>();
        services.AddHostedService<MigrateDbHostedService>();
        
        BsonMapperConfigurate();
        return services;
    }


    private static void BsonMapperConfigurate()
    {
        // BsonClassMap.RegisterClassMap<Car>(x =>
        // {
        //     x.AutoMap();
        //     x.GetMemberMap(c => c.Key).SetIgnoreIfDefault(true);
        // });
        
        BsonClassMap.RegisterClassMap<CarSmall>(cm =>
        {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);
            //cm.UnmapMember(m => m.IsOwner);
        });
    }
}