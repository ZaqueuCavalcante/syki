namespace Syki.Back.Events;

public static class DomainEventMapper
{
    public static string ToDomainEventDescription(this string value)
    {
        if (value.IsEmpty()) return value;

        var type = typeof(IDomainEvent).Assembly.GetType(value)!;
        var customAttributes = (DomainEventAttribute[])type.GetCustomAttributes(typeof(DomainEventAttribute), true);

        return customAttributes[0].Description;
    }

    public static string ToDomainEventEntityName(this string value)
    {
        if (value.IsEmpty()) return value;

        var type = typeof(IDomainEvent).Assembly.GetType(value)!;
        var customAttributes = (DomainEventAttribute[])type.GetCustomAttributes(typeof(DomainEventAttribute), true);

        return customAttributes[0].Entity;
    }
}
