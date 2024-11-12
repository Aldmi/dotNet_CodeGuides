using Domain.Core.FilmsAgregate;
using Domain.Core.StoreAgregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Ef.Configuration;

internal class RentalConfiguration : IEntityTypeConfiguration<Rental>
{
    public void Configure(EntityTypeBuilder<Rental> builder)
    {
        builder= builder.ToTable("rental", schema: "public");
        
        builder.Property(p => p.RentalId).HasColumnName("rental_id");
        builder.HasKey(a => a.RentalId);
        
        
        builder.Property(p => p.RentalDate).HasColumnName("rental_date");
        builder.Property(p => p.ReturnDate).HasColumnName("return_date");
        builder.Property(p => p.LastUpdate).HasColumnName("last_update");
        
        
        builder
            .HasOne(f=>f.Inventory)
            .WithMany()                       
            .HasForeignKey("inventory_id");
        
        builder
            .HasOne(f=>f.Customer)
            .WithMany()                       
            .HasForeignKey("customer_id");
        
        builder
            .HasOne(f=>f.Staff)
            .WithMany()                       
            .HasForeignKey("staff_id");
    }
}