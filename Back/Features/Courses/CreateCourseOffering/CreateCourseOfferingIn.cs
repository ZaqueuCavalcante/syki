using Syki.Back.Domain.Enums;

namespace Syki.Back.Features.Courses.CreateCourseOffering;

public class CreateCourseOfferingIn : IApiDto<CreateCourseOfferingIn>
{
    public int CampusId { get; set; }
    public int CourseId { get; set; }
    public int CourseCurriculumId { get; set; }
    public int AcademicPeriodId { get; set; }
    public CourseSession? CourseSession { get; set; }

    public static IEnumerable<(string, CreateCourseOfferingIn)> GetExamples() =>
    [
        ("Exemplo", new() { }),
    ];
}
