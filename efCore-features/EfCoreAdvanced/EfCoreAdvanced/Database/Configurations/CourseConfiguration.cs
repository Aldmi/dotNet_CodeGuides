using EfCoreAdvanced.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCoreAdvanced.Database.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("Courses").HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("courseId");

        builder.Property(s => s.Name).HasMaxLength(100);

        builder.HasData(Course.AllCourses);
    }
}