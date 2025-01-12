using Ef_Paging.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Database;

public class ApplicationDbContext(IConfiguration configuration) : DbContext
{
    public DbSet<Product> Products { get; set; }
    
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
        modelBuilder.Entity<Product>(p =>
        {
            p.Property(x => x.Name).IsRequired();
            p.HasData(
                new Product { ProductId = Guid.NewGuid(), Name  = "Alex"},
                new Product { ProductId = Guid.NewGuid(), Name  = "Peter" },
                new Product { ProductId = Guid.NewGuid(), Name  = "Elen" },
                new Product { ProductId = Guid.NewGuid(), Name  = "George" },
                new Product { ProductId = Guid.NewGuid(), Name  = "Kate" },
                new Product { ProductId = Guid.NewGuid(), Name  = "Vlad" },
                new Product { ProductId = Guid.NewGuid(), Name  = "Valerian" },
                new Product { ProductId = Guid.NewGuid(), Name  = "Roma" },
                new Product { ProductId = Guid.NewGuid(), Name  = "Kostya" },
                new Product { ProductId = Guid.NewGuid(), Name  = "Bill" },
                
                new Product { ProductId = Guid.NewGuid(), Name  = "Artem" },
                new Product { ProductId = Guid.NewGuid(), Name  = "Agomii" },
                new Product { ProductId = Guid.NewGuid(), Name  = "Agron" }
                );
        });
    }

    private static ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder =>
        {
            // builder.AddFilter((category, level) =>
            //         category == DbLoggerCategory.Database.Command.Name &&
            //         level == LogLevel.Information)
            //     .AddConsole();
        });
}