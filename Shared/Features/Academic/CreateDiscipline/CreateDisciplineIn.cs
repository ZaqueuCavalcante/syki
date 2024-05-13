namespace Syki.Shared;

public class CreateDisciplineIn
{
    public string Name { get; set; }
    public List<Guid> Courses { get; set; } = [];
}
