using EfCoreAdvanced.Entities;
using Microsoft.EntityFrameworkCore;

namespace EfCoreAdvanced.Database;

public class ApplicationDbContext(IConfiguration configuration) : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }

    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            .UseLoggerFactory(CreateLoggerFactory())
            .EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
    
    public ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); }); //в секцию настроек дефолтного логгера добавить ограничения  "Microsoft.EntityFrameworkCore.Database" : "Information"
}