using Microsoft.EntityFrameworkCore;
using WebApi.Domain;

namespace WebApi.Infrastructure.Persistence.Pg;

public sealed class ApplicationDbContext : DbContext
{
    public required DbSet<Product> Products { get; set; }
    

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated();   // создаем базу данных при первом обращении
    }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().Property(p => p.Name).HasMaxLength(1024);
        
        modelBuilder.Entity<Product>().HasData(
             new Product
             {
                 Id = Guid.NewGuid(),
                 Name = "Tv",
                 Price = 5678,
                 IsSaleEnabled = true,
                 StockCount = 0
             },
             new Product
             {
                 Id = Guid.NewGuid(),
                 Name = "Milk",
                 Price = 56,
                 IsSaleEnabled = true,
                 StockCount = 100
             },
             new Product
             {
                 Id = Guid.NewGuid(),
                 Name = "Snickers",
                 Price = 45,
                 IsSaleEnabled = false,
                 StockCount = 6
                 
             }
        );
    }
}