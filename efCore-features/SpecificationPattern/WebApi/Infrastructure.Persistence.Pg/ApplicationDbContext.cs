using Microsoft.EntityFrameworkCore;
using WebApi.Domain;
using WebApi.Domain.Entities.Models;

namespace WebApi.Infrastructure.Persistence.Pg;

public sealed class ApplicationDbContext : DbContext
{
    public required DbSet<Product> Products { get; set; }
    public required DbSet<Category> Categories { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated(); // создаем базу данных при первом обращении
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().Property(p => p.Name).HasMaxLength(1024);
        
        var appliancesCategory = new Category()
        {
            Id = Guid.NewGuid(),
            Name = "Appliances"
        };
        
        var foodCategory = new Category()
        {
            Id = Guid.NewGuid(),
            Name = "Food"
        };

        modelBuilder.Entity<Category>().HasData(
            appliancesCategory,
            foodCategory
            );

        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Tv",
                Price = 5678,
                IsSaleEnabled = true,
                StockCount = 0,
                CategoryId = appliancesCategory.Id
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Milk",
                Price = 56,
                IsSaleEnabled = true,
                StockCount = 100,
                CategoryId = foodCategory.Id
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Snickers",
                Price = 45,
                IsSaleEnabled = false,
                StockCount = 6,
                CategoryId = foodCategory.Id
            }
        );
    }
}