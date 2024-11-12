using Domain.Core.FilmsAgregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NpgsqlTypes;

namespace Persistance.Ef.Configuration;

internal class FilmConfiguration : IEntityTypeConfiguration<Film>
{
    public void Configure(EntityTypeBuilder<Film> builder)
    {
        builder= builder.ToTable("film", schema: "public");
        
        builder.Property(p => p.FilmId).HasColumnName("film_id");
        builder.HasKey(a => a.FilmId);
        
        builder.Property(p => p.Title).HasColumnName("title").HasMaxLength(255);
        builder.Property(p => p.Description).HasColumnName("description");
        builder.Property(p => p.ReleaseYear).HasColumnName("release_year");  // ((VALUE >= 1901) AND (VALUE <= 2155));
        builder.Property(p => p.RentalDuration).HasColumnName("rental_duration");
        builder.Property(p => p.RentalRate).HasColumnName("rental_rate");
        builder.Property(p => p.Lenght).HasColumnName("length");
        builder.Property(p => p.ReplacementCost).HasColumnName("replacement_cost");
        builder.Property(p => p.MpaaRating).HasColumnName("rating").HasConversion(
            v=> MpaaRatingConvertToProvider(v),
            v=> MpaaRatingConvertFromProvider(v));
        builder.Property(p => p.LastUpdate).HasColumnName("last_update").HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(p => p.SpecialFeatures).HasColumnName("special_features");
        builder.Property(p => p.Fulltext).HasColumnName("fulltext");
        
        builder
            .HasOne(f=>f.Language)
            .WithMany()                         //явно не указываю List<Film> в сущности Language
            .HasForeignKey("language_id");
        
        
        // //1. Связь M2M без сущности для связи - film_actor.
        // builder
        //     .HasMany(f=>f.Actors)
        //     .WithMany(a => a.Films)
        //     .UsingEntity(
        //         "film_actor",
        //         l => l.HasOne(typeof(Actor)).WithMany().HasForeignKey("actor_id"),
        //         r => r.HasOne(typeof(Film)).WithMany().HasForeignKey("film_id"),
        //         j => j.HasKey("actor_id", "film_id"));
        
        
        //2. Связь M2M с использованием сущности для связи - FilmActor. И дополнительную нагрузку LastUpdate.
        builder
            .HasMany(f=>f.Actors)
            .WithMany(a=>a.Films)
            .UsingEntity<Film_Actor>(
                "film_actor",
                l => l.HasOne<Actor>(e=>e.Actor).WithMany(e=>e.FilmActors).HasForeignKey("actor_id"),
                r => r.HasOne<Film>(e=>e.Film).WithMany(e=>e.FilmActors).HasForeignKey("film_id"),
                j =>
                {
                    j.HasKey("actor_id", "film_id");
                    j.Property(e => e.LastUpdate).HasColumnName("last_update");
                });
        
        
        builder
            .HasMany(f=>f.Categories)
            .WithMany()                                 //Можно не указывать List<Film> и List<Film_Category> в Category
            .UsingEntity<Film_Category>(
                "film_category",
                l => l.HasOne<Category>(e=>e.Category).WithMany().HasForeignKey("category_id"),
                r => r.HasOne<Film>(e=>e.Film).WithMany(e=>e.FilmCategories).HasForeignKey("film_id"),
                j =>
                { 
                    j.HasKey("category_id", "film_id");
                    j.Property(e => e.LastUpdate).HasColumnName("last_update");
                });
    }
    
    private string MpaaRatingConvertToProvider(MpaaRating mpaaRating)=> mpaaRating switch {
            MpaaRating.G => "G",
            MpaaRating.Pg => "PG",
            MpaaRating.Pg13 => "PG-13",
            MpaaRating.R => "R",
            MpaaRating.Nc17 => "NC-17",
            _ => throw new ArgumentOutOfRangeException(nameof(mpaaRating), mpaaRating, null)
        };
    
    
    private MpaaRating MpaaRatingConvertFromProvider(string str)=> str switch {
            "G"   => MpaaRating.G,
            "PG"  => MpaaRating.Pg,
            "PG-13"  =>MpaaRating.Pg13,
            "R"  => MpaaRating.R,
            "NC-17"  =>MpaaRating.Nc17,
            _ => throw new ArgumentOutOfRangeException(nameof(str), str, null)
        };
}