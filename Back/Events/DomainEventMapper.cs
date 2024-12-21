namespace Syki.Back.Events;

public static class DomainEventMapper
{
    public static string ToDomainEventDescription(this string value)
    {
        if (value.IsEmpty()) return value;

        var type = typeof(IDomainEvent).Assembly.GetType(value)!;
        var customAttributes = (DomainEventDescriptionAttribute[])type.GetCustomAttributes(typeof(DomainEventDescriptionAttribute), true);

        return customAttributes[0].Description;
    }
}
