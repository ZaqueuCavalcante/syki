namespace Syki.Back.Events;

public interface IDomainEventHandler<T> where T : IDomainEvent
{
    Task Handle(Guid institutionId, DomainEventId eventId, T evt);
}
