using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MongoDbCRUD._1_Domain.Entities;
using MongoDbCRUD._2_Persistence.Interfaces;
using MongoDbCRUD._4_Persistence.MogoDb;
using MongoDBMigrations;
using Tests.Modules.Fixtures;
using Tests.Modules.Migration.MogoDb.Tests.Migrations;
using Xunit;
using Version = MongoDBMigrations.Version;

namespace Tests.Modules.Migration.MogoDb.Tests;

[Collection("SharedDbCollection")]
public class CarCollectionsMigrationTests
{
    public CarCollectionsMigrationTests(DatabaseFixture fixture)
    {
        Fixture = fixture;
        CarRepository = new CarRepository(Fixture.Cars, Fixture.ConnectionThrottlingPipeline);
        Fixture.Cleanup();
    }

    private DatabaseFixture Fixture { get; }
    private ICarRepository CarRepository { get; }
    
    
    [Fact]
    public void GetAll()
    {
        try
        {
            var target = new Version(1,0,0);
            //Создается пустая коллекция миграций в БД
            var migrationRunner = new MigrationEngine()
                .UseDatabase(DatabaseFixture.ConnectionString, DatabaseFixture.DbName)
                .UseAssemblyOfType<_1_1_0_SecondMigration>()
                .UseSchemeValidation(false); 

          var res=  migrationRunner.Run(); //Если target не указывать, будут применятся миграции всех след. миграций. исходя из примененных миграция
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
    }
}