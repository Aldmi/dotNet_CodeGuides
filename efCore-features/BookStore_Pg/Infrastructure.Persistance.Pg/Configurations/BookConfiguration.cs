using Domain.Books.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Pg.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books").HasKey(b => b.Id);
        builder.Property(b => b.Id).HasColumnName("bookId");

        builder.Property(a => a.Title).HasMaxLength(250);
        builder.Property(a => a.Description).HasMaxLength(250);
        builder.Property(a => a.PublishedOn).HasColumnName("publishedOn");
        builder.Property(a => a.Publisher).HasMaxLength(150);

        //02O
        builder
            .HasOne(b => b.Promotion)
            .WithOne()
            .HasForeignKey<PriceOffer>(p => p.BookId);
        
        //O2M
        builder
            .HasMany(b => b.Reviews)
            .WithOne()
            .HasForeignKey(r => r.BookId);
        
        //M2M (Повторили свзяь из TagConfiguration)
        builder
            .HasMany(b => b.Tags)
            .WithMany(t => t.Books)
            .UsingEntity(
                "BookTag",
                r => r.HasOne(typeof(Book)).WithMany().HasForeignKey("book_id"),
                l => l.HasOne(typeof(Tag)).WithMany().HasForeignKey("tag_id"), 
                j => j.HasKey("book_id", "tag_id"));
        
        //O2M
        builder
            .HasMany(b => b.AuthorsLink)
            .WithOne(ba => ba.Book)
            .HasForeignKey(ba => ba.BookId);
    }
}