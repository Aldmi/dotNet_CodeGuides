using Domain.Books.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Pg.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("Tags").HasKey(s => s.TagId);
        builder.Property(s => s.TagId).HasMaxLength(100);

        //Не обязательно указывать связь M2M (таблица BookTag создастся автоматом)
        builder
            .HasMany(t => t.Books)
            .WithMany(b => b.Tags)
            .UsingEntity(
                "BookTag",
                r => r.HasOne(typeof(Book)).WithMany().HasForeignKey("book_id"),
                l => l.HasOne(typeof(Tag)).WithMany().HasForeignKey("tag_id"), 
                j => j.HasKey("book_id", "tag_id"));
    }
}