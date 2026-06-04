namespace Syki.Back.Features.Students.EnrollStudentInCourseOffering;

public class EnrollStudentInCourseOfferingOut : IApiDto<EnrollStudentInCourseOfferingOut>
{
    public int Id { get; set; }

    public static IEnumerable<(string, EnrollStudentInCourseOfferingOut)> GetExamples() =>
    [
        ("Exemplo", new() { Id = 1 }),
    ];
}
