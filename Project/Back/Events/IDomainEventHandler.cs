namespace Exato.Back.Events;

public interface IDomainEventHandler<T> where T : IDomainEvent
{
    Task Handle(int organizationId, Guid eventId, T evt);
}
