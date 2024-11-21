using Infrastructure.Persistance.Pg;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TestSupport.Helpers;

namespace Test;

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
        var connection = GetConnectionString();
        var context = new BookContext(connection!);
        return context;
    }
    
    
    public BookContext DbContextFactoryWithSqlLogs(Action<string> logTo, LogLevel logLevel = LogLevel.Information)
    {
        var connection = GetConnectionString();
        var context = new BookContext(connection!, null, logTo, logLevel);
        return context;
    }

    private string? GetConnectionString()
    {
       var configuration = AppSettings.GetConfiguration();
       return configuration.GetConnectionString("BookStore_Pg_Connection_Str");
    }
}

[CollectionDefinition("Database collection")]
public class DatabaseCollection : ICollectionFixture<DbContextFixture> { }