namespace DeterminingTypeFromString.Messages;

public record CustomerCreated : IMessage
{
   public required string FullName { get; init; }
   public required string Email { get; init; }
   public required string GitHubUserName { get; init; }
   public required DateOnly DateOfBirth { get; init; }
}