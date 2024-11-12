using Domain.Core.AddressAgregate;
using Domain.Core.CustomerAgregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Ef.Configuration;

internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder= builder.ToTable("customer", schema: "public");
        
        builder.Property(p => p.CustomerId).HasColumnName("customer_id");
        builder.HasKey(a => a.CustomerId);
     
        builder.Property(p => p.StoreId).HasColumnName("store_id");
        builder.Property(p => p.FirstName).HasColumnName("first_name").HasMaxLength(45);
        builder.Property(p => p.LastName).HasColumnName("last_name").HasMaxLength(45);
        builder.Property(p => p.Email).HasColumnName("email").HasMaxLength(50);
        builder.Property(p => p.ActiveBool).HasColumnName("activebool");
        builder.Property(p => p.CreateDate).HasColumnName("create_date");
        builder.Property(p => p.LastUpdate).HasColumnName("last_update");
        builder.Property(p => p.Active).HasColumnName("active");
        
        builder
            .HasOne<Address>(c=>c.Address)
            .WithMany()
            .HasForeignKey("address_id");
    }
}