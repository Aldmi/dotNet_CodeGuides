using CSharpFunctionalExtensions;

namespace EfCoreAdvanced.Entities;

public class Course : Entity
{
    public static readonly Course Math = new(1, "Math");
    public static readonly Course Chemistry = new(2, "Chemistry");
    
    public static readonly Course[] AllCourses = [Math, Chemistry];

    public string Name { get; } = null!;


    private Course(long id, string name)
        : base(id)
    {
        Name = name;
    }
    
    private Course() { }
    
    
    public static Result<Course> FromId(long id)
    {
        var course = AllCourses.SingleOrDefault(c => c.Id == id);
        return course ?? Result.Failure<Course>("Course not found");
    }
}