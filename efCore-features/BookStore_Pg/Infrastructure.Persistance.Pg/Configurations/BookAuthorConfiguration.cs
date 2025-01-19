using Domain.Books.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Pg.Configurations;

public class BookAuthorConfiguration : IEntityTypeConfiguration<BookAuthor>
{
    public void Configure(EntityTypeBuilder<BookAuthor> builder)
    {
        builder.HasKey(x => new {x.BookId, x.AuthorId});
        
        builder.Property(a => a.Order).HasColumnName("order").HasColumnType("smallint");
    }
}