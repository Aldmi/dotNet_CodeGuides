using Domain.Core.AddressAgregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Ef.Configuration;


internal class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder= builder.ToTable("city", schema: "public");
        
        builder.Property(p => p.CityId).HasColumnName("city_id");
        builder.HasKey(a => a.CityId);
     
        builder.Property(p => p.Name).HasColumnName("city").HasMaxLength(50);
        
        builder.Property(p => p.LastUpdate).HasColumnName("last_update");
        

        //1. Полная конфигурация связей (CountryIdFk - указан явно)
        // builder.Property(p => p.CountryIdFk).HasColumnName("country_id");
        //  builder
        //      .HasOne(city => city.Country)
        //      .WithMany(country => country.Cities)
        //      .HasForeignKey(u => u.CountryIdFk);
        
        
        //2. Теневое свойство (country_id).
        //Country - навигационное свойство в объекте City
        // builder
        //     .HasOne(city => city.Country)
        //     .WithMany(country => country.Cities)
        // .HasForeignKey("country_id");
        
        
        //3. Теневое свойство (country_id).
        //не задаем Country в объекте City
        builder
            .HasOne<Country>()
            .WithMany(country => country.Cities)
            .HasForeignKey("country_id");
    }
}