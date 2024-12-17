using Syki.Back.Events;

namespace Syki.Tests.Base;

public static class DomainEventsExtensions
{
    public static T ShouldPublishDomainEvent<T>(this Entity entity) where T : IDomainEvent
    {
        var domainEvent = entity.GetDomainEvents().OfType<T>().FirstOrDefault();

        if (domainEvent == null)
        {
            throw new Exception($"{typeof(T).Name} not published");
        }

        return domainEvent;
    }
}
