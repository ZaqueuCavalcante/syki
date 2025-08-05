namespace Syki.Shared;

public class CreateCourseOfferingIn
{
    public Guid CampusId { get; set; }
    public Guid CourseId { get; set; }
    public Guid CourseCurriculumId { get; set; }
    public string? Period { get; set; }
    public Shift Shift { get; set; }

    public static IEnumerable<(string, CreateCourseOfferingIn)> GetExamples() =>
    [
        ("Exemplo", new() { }),
    ];
}
