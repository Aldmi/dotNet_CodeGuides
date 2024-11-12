using Domain.Core.AddressAgregate;
using Domain.Core.CustomerAgregate;
using Domain.Core.StaffAgregate;
using Domain.Core.StoreAgregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Ef.Configuration;

internal class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder= builder.ToTable("payment", schema: "public");
        
        builder.Property(p => p.PaymentId).HasColumnName("payment_id");
        builder.HasKey(a => a.PaymentId);
        
        builder.Property(p => p.PaymentDate).HasColumnName("payment_date");
        builder.Property(p => p.Amount).HasColumnName("amount");
        
        builder
            .HasOne(c=>c.Staff)
            .WithMany()
            .HasForeignKey("staff_id");
        
        builder
            .HasOne(c=>c.Rental)
            .WithMany()
            .HasForeignKey("rental_id");
        
        builder
            .HasOne(c=>c.Customer)
            .WithMany()
            .HasForeignKey("customer_id");
    }
}