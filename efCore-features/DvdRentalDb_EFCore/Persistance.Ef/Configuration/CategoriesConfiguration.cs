using Domain.Core.FilmsAgregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Ef.Configuration;


internal class CategoriesConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder= builder.ToTable("category", schema: "public");
        
        builder.Property(p => p.CategoryId).HasColumnName("category_id");
        builder.HasKey(a => a.CategoryId);
     
        builder.Property(p => p.Name).HasColumnName("name").HasMaxLength(25);
        
        builder.Property(p => p.LastUpdate).HasColumnName("last_update");
    }
}