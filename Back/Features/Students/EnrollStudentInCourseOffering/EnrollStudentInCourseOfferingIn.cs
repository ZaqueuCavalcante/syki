namespace Syki.Back.Features.Students.EnrollStudentInCourseOffering;

public class EnrollStudentInCourseOfferingIn : IApiDto<EnrollStudentInCourseOfferingIn>
{
    public int CourseOfferingId { get; set; }

    public static IEnumerable<(string, EnrollStudentInCourseOfferingIn)> GetExamples() =>
    [
        ("Exemplo", new() { CourseOfferingId = 1 }),
    ];
}
