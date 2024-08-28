namespace Syki.Back.Database;

interface DomainEventsPublisher
{
    public List<string> DomainEvents { get; set; }

    public void Publish(string domainEvent)
    {
        DomainEvents ??= [];
        DomainEvents.Add(domainEvent);
    }
}
