using Domain.Core.FilmsAgregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Ef.Configuration;

internal class ActorsConfiguration : IEntityTypeConfiguration<Actor>
{
    public void Configure(EntityTypeBuilder<Actor> builder)
    {
        builder= builder.ToTable("actor", schema: "public");
        
        builder.Property(p => p.ActorId).HasColumnName("actor_id");
        builder.HasKey(a => a.ActorId);
     
        builder.Property(p => p.FirstName).HasColumnName("first_name").HasMaxLength(45);
     
        builder.Property(p => p.LastName).HasColumnName("last_name").HasMaxLength(45);;
        
        builder.Property(p => p.LastUpdate).HasColumnName("last_update");
    }
}