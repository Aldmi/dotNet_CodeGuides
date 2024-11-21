using Application.Core.Abstract;
using CSharpFunctionalExtensions;
using Domain.Books.Entities;
using Domain.Books.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistance.Pg;

public class BookContext(string connectionStr, IPublisher? _publisher= null, Action<string>? logTo = null, LogLevel logLevel = LogLevel.Information)
    : DbContext, IBookContext
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
        // modelBuilder.Entity<BookView>()
        //     .ToView("Books");
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookContext).Assembly);
    }


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        int result= await base.SaveChangesAsync(cancellationToken);
        if (_publisher is not null)
        {
            await PublishDomainEventAsync();
        }
        return result;
    }

    private async Task PublishDomainEventAsync()
    {
        var domainEvents = ChangeTracker
            .Entries<DomainEntity>()
            .Select(e => e.Entity)
            .SelectMany(e =>
            {
                List<IDomainEvent> domainEvents = e.DomainEvents;
                e.ClearDomainEvents();
                return domainEvents;
            }).ToList();

        foreach (var domainEvent in domainEvents)
        {
            await _publisher!.Publish(domainEvent);
        }
    }
}