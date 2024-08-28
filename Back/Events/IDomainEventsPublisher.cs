namespace Syki.Back.Events;

interface IDomainEventsPublisher
{
    public List<DomainEvent> DomainEvents { get; set; }
}
