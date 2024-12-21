namespace Syki.Back.Events;

public interface IDomainEventHandler<T> where T : IDomainEvent
{
    Task Handle(Guid eventId, T task);
}
