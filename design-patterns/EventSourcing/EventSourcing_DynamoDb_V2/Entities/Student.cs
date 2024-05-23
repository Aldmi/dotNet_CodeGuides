using EventSourcing_DynamoDb_V2.Events;
using EventSourcing_DynamoDb.Events;

namespace EventSourcing_DynamoDb_V2.Entities;

public class Student
{
    public Guid Id { get; set; }
    
    public string FullName { get; set; }
    
    public string Email { get; set; }

    public List<string> EnrolledCourses { get; set; } = [];
    
    public DateTime DateOfBirth { get; set; }


    private void Apply(StudentCreated studentCreated)
    {
        Id = studentCreated.StudentId;
        FullName = studentCreated.FullName;
        Email = studentCreated.Email;
        DateOfBirth = studentCreated.DateOfBirth;
    }
    
    private void Apply(StudentUpdated studentUpdated)
    {
        FullName = studentUpdated.FullName;
        Email = studentUpdated.Email;
    }
    
    private void Apply(StudentEnrolled studentEnrolled)
    {
        if (!EnrolledCourses.Contains(studentEnrolled.CourseName))
        {
            EnrolledCourses.Add(studentEnrolled.CourseName);
        }
    }
    
    private void Apply(StudentUnEnrolled studentUnEnrolled)
    {
        if (EnrolledCourses.Contains(studentUnEnrolled.CourseName))
        {
            EnrolledCourses.Remove(studentUnEnrolled.CourseName);
        }
    }


    public void Apply(Event @event)
    {
        switch (@event)
        {
            case StudentCreated studentCreated:
                Apply(studentCreated);
                break;
            case StudentEnrolled studentEnrolled:
                Apply(studentEnrolled);
                break;
            case StudentUnEnrolled studentUnEnrolled:
                Apply(studentUnEnrolled);
                break;
            case StudentUpdated studentUnEnrolled:
                Apply(studentUnEnrolled);
                break;
        }
    }
}