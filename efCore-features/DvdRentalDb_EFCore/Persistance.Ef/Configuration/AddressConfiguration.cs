using Domain.Core.AddressAgregate;
using Domain.Core.FilmsAgregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Ef.Configuration;

internal class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder= builder.ToTable("address", schema: "public");
        
        builder.Property(p => p.AddressId).HasColumnName("address_id");
        builder.HasKey(a => a.AddressId);
     
        builder.Property(p => p.Address1).HasColumnName("address").HasMaxLength(50);
        builder.Property(p => p.Address2).HasColumnName("address2").HasMaxLength(50);
        builder.Property(p => p.District).HasColumnName("district").HasMaxLength(20);
        builder.Property(p => p.PostalCode).HasColumnName("postal_code").HasMaxLength(10);
        builder.Property(p => p.Phone).HasColumnName("phone").HasMaxLength(20);
        builder.Property(p => p.LastUpdate).HasColumnName("last_update");
        
        builder
            .HasOne<City>(e => e.City)
            .WithMany()
            .HasForeignKey("city_id");
    }
}