namespace Syki.Back.Features.Courses.CreateCourseOffering;

public class CreateCourseOfferingOut : IApiDto<CreateCourseOfferingOut>
{
    public int Id { get; set; }

    public static IEnumerable<(string, CreateCourseOfferingOut)> GetExamples() =>
    [
        ("Exemplo", new() { Id = 1 }),
    ];
}
