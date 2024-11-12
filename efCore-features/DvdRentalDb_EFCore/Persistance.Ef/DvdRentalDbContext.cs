using Domain.Core.AddressAgregate;
using Domain.Core.CustomerAgregate;
using Domain.Core.FilmsAgregate;
using Domain.Core.StaffAgregate;
using Domain.Core.StoreAgregate;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Ef;

public class DvdRentalDbContext : DbContext
{
    public DvdRentalDbContext(DbContextOptions<DvdRentalDbContext> options)
        : base(options)
    {
        // Database.EnsureCreated();   // создаем базу данных при первом обращении
    }
    
    public required DbSet<Actor> Actors { get; set; }
    public required DbSet<Category> Categories { get; set; }
    public required DbSet<Country> Countries { get; set; }
    public required DbSet<City> Citys { get; set; }
    public required DbSet<Address> Addresses { get; set; }
    public required DbSet<Film> Films { get; set; }
    public required DbSet<Customer> Customers { get; set; }
    public required DbSet<Staff> Staffs { get; set; }
    public required DbSet<Inventory> Inventories { get; set; }
    public required DbSet<Store> Stores  { get; set; }
    public required DbSet<Rental> Rentals  { get; set; }
    public required DbSet<Payment> Payments  { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var configAssembly = typeof(DvdRentalDbContext).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(configAssembly);
    }
}