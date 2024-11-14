using Domain.Books.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistance.Pg;

public class BookContext(string connectionStr, Action<string>? logTo = null, LogLevel logLevel = LogLevel.Information)
    : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<PriceOffer> PriceOffers { get; set; }
    public DbSet<BookView> BookViews { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = optionsBuilder.UseNpgsql(connectionStr);
        if (logTo is not null)
        {
            builder.LogTo(logTo, LogLevel.Information).EnableSensitiveDataLogging();
        }
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookAuthor>() 
            .HasKey(x => new {x.BookId, x.AuthorId});
        
        // modelBuilder.Entity<BookView>()
        //     .ToView("Books");
    }
}