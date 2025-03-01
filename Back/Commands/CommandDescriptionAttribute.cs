namespace Syki.Back.Commands;

[AttributeUsage(AttributeTargets.Class)]
public class CommandDescriptionAttribute(string description) : Attribute
{
    public string Description { get; set; } = description;
}
