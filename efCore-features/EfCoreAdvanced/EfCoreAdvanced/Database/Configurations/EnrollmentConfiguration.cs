using EfCoreAdvanced.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCoreAdvanced.Database.Configurations;

public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.ToTable("Enrollments").HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("enrollmentId");

        builder.Property(e => e.Grade);
        
        //У зачисления один студент, у студента много зачислений.
        builder
            .HasOne(e => e.Student)
            .WithMany(s => s.Enrollments);
            
        //У зачисления один курс, у курса много зачислений.
        builder
            .HasOne(e => e.Course)
            .WithMany(); // у курса нет навигационного свойства к зачислению (зачислениями должен управлять студент)
    }
}