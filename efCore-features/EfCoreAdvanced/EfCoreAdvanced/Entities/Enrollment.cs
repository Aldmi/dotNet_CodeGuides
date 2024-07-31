using CSharpFunctionalExtensions;

namespace EfCoreAdvanced.Entities;

public class Enrollment : Entity
{
    public Grade Grade { get; }
    public Course Course { get; } = null!;
    public Student Student { get; } = null!;

    public Enrollment(Course course, Student student, Grade grade)
    {
        Course = course;
        Student = student;
        Grade = grade;
    }

    private Enrollment() { }
}