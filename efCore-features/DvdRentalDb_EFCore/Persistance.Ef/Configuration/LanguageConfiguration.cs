using Domain.Core.FilmsAgregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Ef.Configuration;


internal class LanguageConfiguration : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder= builder.ToTable("language", schema: "public");
        
        builder.Property(p => p.LanguageId).HasColumnName("language_id");
        builder.HasKey(a => a.LanguageId);
     
        builder.Property(p => p.Name).HasColumnName("name").HasMaxLength(20);
        
        builder.Property(p => p.LastUpdate).HasColumnName("last_update");
        
        

        
    }
}