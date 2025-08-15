namespace Syki.Shared;

public class GetCampusTeachersOut
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<Guid> Disciplines { get; set; }
}
