using Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Pg;

public sealed class ApplicationDbContext : DbContext
{
    public required DbSet<Persone> Persones { get; set; }
    

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
       // Database.EnsureCreated();   // создаем базу данных при первом обращении
    }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Persone>().Property(p => p.Id).HasDefaultValueSql("gen_random_uuid()");
        modelBuilder.Entity<Persone>().Property(p => p.Name).HasMaxLength(1024);

        modelBuilder.Entity<Persone>().OwnsOne<Email>(p => p.Email);
        modelBuilder.Entity<Persone>().OwnsMany(p => p.Cars);
        
        
        modelBuilder.Entity<Address>().Property(p => p.Id).HasDefaultValueSql("gen_random_uuid()");
        modelBuilder.Entity<Address>().Property(p => p.Country).HasMaxLength(1024);
        modelBuilder.Entity<Address>().Property(p => p.City).HasMaxLength(1024);
        
        



        // modelBuilder.Entity<Persone>().HasData(
        //      new Persone
        //      {
        //          Id = Guid.NewGuid(),
        //          Name = "Alex",
        //          Age = 18,
        //          Country = "Russia",
        //          City = "Moscow"
        //      },
        //      new Persone
        //      {
        //          Id = Guid.NewGuid(),
        //          Name = "Peter",
        //          Age = 28,
        //          Country = "England",
        //          City = "Bermangham"
        //      },
        //      new Persone
        //      {
        //          Id = Guid.NewGuid(),
        //          Name = "Jone",
        //          Age = 58,
        //          Country = "USA",
        //          City = "Washington"
        //      }
        // );
    }
}