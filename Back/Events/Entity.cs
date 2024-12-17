namespace Syki.Back.Events;

public abstract class Entity
{
    private List<IDomainEvent> _domainEvents;

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents ??= [];
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }

    public List<IDomainEvent> GetDomainEvents()
    {
        return _domainEvents ?? [];
    }
}
