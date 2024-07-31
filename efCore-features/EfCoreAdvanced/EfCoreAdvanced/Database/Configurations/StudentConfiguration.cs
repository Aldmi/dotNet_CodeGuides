using EfCoreAdvanced.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCoreAdvanced.Database.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students").HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("studentId");

        builder.ComplexProperty(s => s.Name, nameBuilder =>
        {
            nameBuilder.Property(n => n.First).HasColumnName("FirstName").HasMaxLength(100); //По умолчанию EF задаст имя "Name_First". Лутчше задавать ограничения столбца для оптимизации работы со строками
            nameBuilder.Property(n => n.Last).HasColumnName("LastName").HasMaxLength(100); //По умолчанию EF задаст имя "Last_First".  Лутчше задавать ограничения столбца для оптимизации работы со строками
        });

        builder
            .HasOne(s => s.FavoriteCourse)
            .WithMany(); //Нет навигациолнных свойств на студента

        builder
            .HasMany(s => s.Enrollments);
    }
}