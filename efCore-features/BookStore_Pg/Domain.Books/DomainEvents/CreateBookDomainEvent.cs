using Domain.Books.Primitives;

namespace Domain.Books.DomainEvents;

public sealed record CreateBookDomainEvent(long BookId, string Title) : IDomainEvent
{
}

    
