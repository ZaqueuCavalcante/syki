using Syki.Back.Domain.Enums;

namespace Syki.Back.Shared;

public class CreateCourseOfferingIn : IApiDto<CreateCourseOfferingIn>
{
    public Guid CampusId { get; set; }
    public Guid CourseId { get; set; }
    public Guid CourseCurriculumId { get; set; }
    public string? Period { get; set; }
    public CourseSession? CourseSession { get; set; }

    public static IEnumerable<(string, CreateCourseOfferingIn)> GetExamples() =>
    [
        ("Exemplo", new() { }),
    ];
}
