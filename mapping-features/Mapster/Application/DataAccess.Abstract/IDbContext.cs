using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.DataAccess.Abstract;

public interface IDbContext
{
    DbSet<Product> Products { get; }
    
    Task<int> SaveChangesAsync(CancellationToken token = default);
}