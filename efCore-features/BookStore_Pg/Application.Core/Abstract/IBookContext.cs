using Domain.Books.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Core.Abstract;

//TODO: можно вынести в слой  Infrastructure.Interfaces/DataAccess.Interfaces
public interface IBookContext
{
    DbSet<Book> Books { get; }
    DbSet<Author> Authors { get; }
    DbSet<Tag> Tags { get; }
    DbSet<PriceOffer> PriceOffers { get; }

    Task<int> SaveChangesAsync(CancellationToken token = default);
    
    
    void EnsureDeleted();
    void EnsureCreated();
}