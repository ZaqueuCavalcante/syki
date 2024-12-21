namespace Syki.Back.Tasks;

[AttributeUsage(AttributeTargets.Class)]
public class SykiTaskDescriptionAttribute(string description) : Attribute
{
    public string Description { get; set; } = description;
}
