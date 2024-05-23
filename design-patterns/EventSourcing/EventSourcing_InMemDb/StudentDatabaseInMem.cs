using EventSourcing_InMemDb.Entities;
using EventSourcing_InMemDb.Events;

namespace EventSourcing_InMemDb;

public class StudentDatabaseInMem
{
    /// <summary>
    /// Словарь событий
    /// Key - Ключ для потока событий
    /// Value - Список событий упорядоченных по дате добавления.
    /// </summary>
    private readonly Dictionary<Guid, SortedList<DateTime, Event>> _studentEvents = new ();

    /// <summary>
    /// Словарь для моментальных снимков. Чтобы хранить последнее состояние.
    /// </summary>
    private readonly Dictionary<Guid, Student> _students = new();
    
    
    /// <summary>
    /// Доавить в конец отсортированного списка новое событие
    /// </summary>
    public void Append(Event @event)
    {
        var stream = _studentEvents!.GetValueOrDefault(@event.StreamId, null);
        if (stream is null)
        {
            _studentEvents[@event.StreamId] = new SortedList<DateTime, Event>();
        }
        
        @event.CreatedAtUtc = DateTime.UtcNow;
        
        //В реальной БД, Сохранить событие в БД и Сохранить конечное состояние в БД нужно обернуть в транзакцию для согласованности данных.
        //Сохранить событие в БД
        _studentEvents[@event.StreamId].Add(@event.CreatedAtUtc, @event);
        //Сохранить конечное состояние в БД
        _students[@event.StreamId] = GetStudent(@event.StreamId)!;
    }

    /// <summary>
    /// Вернуть конечное состояние
    /// </summary>
    public Student? GetStudentView(Guid studentId)
    {
        return _students!.GetValueOrDefault(studentId, null);
    }
    
    
    /// <summary>
    /// Получить студента.
    /// Создается новый студент, потом извлекаются все события связанные с ним из БД и эти события последовательно применяются к студенту.
    /// </summary>
    public Student? GetStudent(Guid studentId)
    {
        if (!_studentEvents.ContainsKey(studentId))
        {
            return null;
        }

        var student = new Student();
        var studentEvents = _studentEvents[studentId];
        foreach (var studentEvent in studentEvents)
        {
            student.Apply(studentEvent.Value);
        }
        return student;
    }
}

     