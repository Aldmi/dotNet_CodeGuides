namespace EventSourcing_InMemDb.Events;

/// <summary>
/// Событие зачисления на курс
/// </summary>
public class StudentEnrolled : Event
{
    public required Guid StudentId { get; init; }
    
    public required string CourseName { get; set; }

    public override Guid StreamId => StudentId;
}