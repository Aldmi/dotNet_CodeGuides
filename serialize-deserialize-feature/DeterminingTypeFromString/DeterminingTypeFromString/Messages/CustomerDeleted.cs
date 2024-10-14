namespace DeterminingTypeFromString.Messages;

public record CustomerDeleted : IMessage
{
    public required Guid Id { get; init; }
}