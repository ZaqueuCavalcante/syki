using System.Collections.Concurrent;

namespace Syki.Back.Events;

public static class DomainEventMapper
{
    private static readonly ConcurrentDictionary<string, DomainEventAttribute> _attributes = new();

    public static string ToDomainEventDescription(this string value)
    {
        if (value.IsEmpty()) return value;

        return _attributes.GetOrAdd(value, GetDomainEventAttribute(value)).Description;
    }

    public static string ToDomainEventEntityName(this string value)
    {
        if (value.IsEmpty()) return value;

        return _attributes.GetOrAdd(value, GetDomainEventAttribute(value)).Entity;
    }

    private static DomainEventAttribute GetDomainEventAttribute(this string value)
    {
        var type = typeof(IDomainEvent).Assembly.GetType(value)!;
        var customAttributes = (DomainEventAttribute[])type.GetCustomAttributes(typeof(DomainEventAttribute), true);
        return customAttributes[0];
    }
}
