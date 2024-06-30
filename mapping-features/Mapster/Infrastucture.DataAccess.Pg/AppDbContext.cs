using Application.DataAccess.Abstract;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastucture.DataAccess.Pg;

public class AppDbContext : DbContext, IDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public required DbSet<Product> Products { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<Product>().HasData(new List<Product>()
        {
            new(Guid.NewGuid(), "Product 1", 10, 562, DateTime.UtcNow),
            new(Guid.NewGuid(), "Product 2", 100, 4158, DateTime.UtcNow),
            new(Guid.NewGuid(), "Product 2", 5469, 25, DateTime.UtcNow),
        });
    }
}