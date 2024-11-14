using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TestSupport.Helpers;

namespace Infrastructure.Persistance.Pg.Tests.EfCoreDbContextTest;

public class DbContextFixture
{
    /// <summary>
    /// Создаем БД перед тестами
    /// </summary>
    public DbContextFixture()
    {
        var dbContext = DbContextFactory();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
    }
    
    
    public BookContext DbContextFactory()
    {
        var configuration = AppSettings.GetConfiguration();
        var connection = configuration.GetConnectionString("BookStore_Pg_Connection_Str");
        var context = new BookContext(connection!);
        return context;
    }
    
    
    public BookContext DbContextFactoryWithSqlLogs(Action<string> logTo, LogLevel logLevel = LogLevel.Information)
    {
        var configuration = AppSettings.GetConfiguration();
        var connection = configuration.GetConnectionString("BookStore_Pg_Connection_Str");
        var context = new BookContext(connection!, logTo, logLevel);
        return context;
    }
}

[CollectionDefinition("Database collection")]
public class DatabaseCollection : ICollectionFixture<DbContextFixture> { }