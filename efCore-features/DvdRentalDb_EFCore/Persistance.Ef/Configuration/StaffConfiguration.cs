using Domain.Core.AddressAgregate;
using Domain.Core.CustomerAgregate;
using Domain.Core.StaffAgregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Ef.Configuration;

internal class StaffConfiguration : IEntityTypeConfiguration<Staff>
{
    public void Configure(EntityTypeBuilder<Staff> builder)
    {
        builder= builder.ToTable("staff", schema: "public");
        
        builder.Property(p => p.StuffId).HasColumnName("staff_id");
        builder.HasKey(a => a.StuffId);
        
        builder.Property(p => p.FirstName).HasColumnName("first_name").HasMaxLength(45);
        builder.Property(p => p.LastName).HasColumnName("last_name").HasMaxLength(45);
        builder.Property(p => p.Email).HasColumnName("email").HasMaxLength(50);
        builder.Property(p => p.StoreId).HasColumnName("store_id");
        builder.Property(p => p.ActiveBool).HasColumnName("active");
        builder.Property(p => p.UserName).HasColumnName("username");
        builder.Property(p => p.Password).HasColumnName("password");
        builder.Property(p => p.LastUpdate).HasColumnName("last_update");
        
        builder.Property(p => p.Picture).HasColumnName("picture");
        
        builder
            .HasOne<Address>(c=>c.Address)
            .WithMany()
            .HasForeignKey("address_id");
    }
}