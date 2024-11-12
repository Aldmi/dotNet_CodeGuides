using Domain.Core.FilmsAgregate;
using Domain.Core.StoreAgregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Ef.Configuration;

internal class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
        builder= builder.ToTable("inventory", schema: "public");
        
        builder.Property(p => p.InventoryId).HasColumnName("inventory_id");
        builder.HasKey(a => a.InventoryId);
     
        builder.Property(p => p.StoreId).HasColumnName("store_id");
        builder.Property(p => p.LastUpdate).HasColumnName("last_update");
        
        builder
            .HasOne(f=>f.Film)
            .WithMany()                       
            .HasForeignKey("film_id");
    }
}