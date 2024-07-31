using CSharpFunctionalExtensions;

namespace EfCoreAdvanced.Entities;

public class Student : Entity
{
    public Name Name { get; set; } = null!;
    public Course? FavoriteCourse { get; set; }

    public List<Enrollment> Enrollments { get; set; } = [];
    
    public Student(Name name, Course? favoriteCourse)
    {
        Name = name;
        FavoriteCourse = favoriteCourse;
    }
    
    private Student() { }
}