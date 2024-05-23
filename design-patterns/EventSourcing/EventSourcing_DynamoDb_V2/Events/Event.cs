using System.Text.Json.Serialization;
using EventSourcing_DynamoDb.Events;

namespace EventSourcing_DynamoDb_V2.Events;

[JsonPolymorphic]
[JsonDerivedType(typeof(StudentCreated), nameof(StudentCreated))]
[JsonDerivedType(typeof(StudentUpdated), nameof(StudentUpdated))]
[JsonDerivedType(typeof(StudentEnrolled), nameof(StudentEnrolled))]
[JsonDerivedType(typeof(StudentUnEnrolled), nameof(StudentUnEnrolled))]
public abstract class Event
{
    public abstract Guid StreamId { get; }
    
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Первичный ключ 
    /// </summary>
    [JsonPropertyName("pk")]
    public string Pk => StreamId.ToString();
    
    /// <summary>
    /// Ключ сортировки
    /// </summary>
    [JsonPropertyName("sk")]
    public string Sk => CreatedAtUtc.ToString("O");
}