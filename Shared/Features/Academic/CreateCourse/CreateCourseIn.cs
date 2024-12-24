namespace Syki.Shared;

public class CreateCourseIn
{
    public string Name { get; set; }
    public required CourseType Type { get; set; }
    public List<string> Disciplines { get; set; } = [];
}
