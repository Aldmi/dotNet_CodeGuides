namespace EventSourcing_InMemDb.Events;

/// <summary>
/// Событие отчисления с курса
/// </summary>
public class StudentUnEnrolled : Event
{
    public required Guid StudentId { get; init; }
    
    public required string CourseName { get; set; }

    public override Guid StreamId => StudentId;
}