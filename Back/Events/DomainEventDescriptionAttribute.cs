namespace Syki.Back.Events;

[AttributeUsage(AttributeTargets.Class)]
public class DomainEventAttribute(string entity, string description) : Attribute
{
    public string Entity { get; set; } = entity;
    public string Description { get; set; } = description;
}
