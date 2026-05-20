using Syki.Back.Domain.Enums;

namespace Syki.Back.Shared;

public class CourseOfferingOut : IApiDto<CourseOfferingOut>
{
    public Guid Id { get; set; }
    public string Campus { get; set; }
    public string Course { get; set; }
    public Guid CourseCurriculumId { get; set; }
    public string CourseCurriculum { get; set; }
    public string Period { get; set; }
    public CourseSession CourseSession { get; set; }

    public static IEnumerable<(string, CourseOfferingOut)> GetExamples() =>
    [
        ("Exemplo", new() { }),
    ];

    public override string ToString()
    {
        return $"{CourseCurriculum} | {Campus} | {Period} | {CourseSession.GetDescription()}";
    }

    public static implicit operator CourseOfferingOut(OneOf<CourseOfferingOut, ErrorOut> value)
    {
        return value.Success;
    }
}
