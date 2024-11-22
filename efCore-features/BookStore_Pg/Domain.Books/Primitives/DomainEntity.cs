using CSharpFunctionalExtensions;

namespace Domain.Books.Primitives;

public class DomainEntity : Entity
{
    private readonly List<IDomainEvent> _domainEvents = new();

    // public AgregateRoot(long id)
    //     : base(id)
    // {
    // }
    
    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);  
    }

    public List<IDomainEvent> DomainEvents => _domainEvents.ToList();


    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}