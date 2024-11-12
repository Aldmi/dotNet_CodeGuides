using Domain.Core.AddressAgregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Ef.Configuration;


internal class CountriesConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder= builder.ToTable("country", schema: "public");
        
        builder.Property(p => p.CountryId).HasColumnName("country_id");
        builder.HasKey(a => a.CountryId);
     
        builder.Property(p => p.Value).HasColumnName("country").HasMaxLength(50);
        
        builder.Property(p => p.LastUpdate).HasColumnName("last_update");
    }
}