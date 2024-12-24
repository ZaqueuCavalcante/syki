namespace Syki.Back.Events;

public interface IDomainEventHandler<T> where T : IDomainEvent
{
    Task Handle(Guid eventId, Guid institutionId, T task);
}
