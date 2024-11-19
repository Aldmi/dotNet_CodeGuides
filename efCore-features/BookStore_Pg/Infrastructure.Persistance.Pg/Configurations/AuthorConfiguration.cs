using Domain.Books.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Pg.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.ToTable("Authors").HasKey(s => s.AuthorId);
        builder.Property(a => a.AuthorId).HasColumnName("authorId");

        builder.Property(a => a.Name).HasMaxLength(100);
        
        builder
            .HasMany(a => a.BooksLink)
            .WithOne(ba => ba.Author)
            .HasForeignKey(ba => ba.AuthorId);
    }
}