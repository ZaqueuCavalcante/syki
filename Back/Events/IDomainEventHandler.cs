namespace Syki.Back.Events;

public interface IDomainEventHandler<T> where T : IDomainEvent
{
    Task Handle(Guid institutionId, Guid eventId, T evt);
}
