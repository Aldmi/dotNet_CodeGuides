using Domain.Core.FilmsAgregate;
using Domain.Core.StoreAgregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Ef.Configuration;

internal class StoreConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder= builder.ToTable("store", schema: "public");
        
        builder.Property(p => p.StoreId).HasColumnName("store_id");
        builder.HasKey(a => a.StoreId);
        
        builder.Property(p => p.LastUpdate).HasColumnName("last_update");
        
        builder
            .HasOne(f=>f.ManagerStaff)
            .WithMany()                       
            .HasForeignKey("manager_staff_id");
        
        builder
            .HasOne(f=>f.Address)
            .WithMany()                       
            .HasForeignKey("address_id");
    }
}