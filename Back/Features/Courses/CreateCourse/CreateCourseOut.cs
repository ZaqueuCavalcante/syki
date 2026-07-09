namespace Estud.Back.Features.Courses.CreateCourse;

public class CreateCourseOut : IApiDto<CreateCourseOut>
{
    public int Id { get; set; }

    public static IEnumerable<(string, CreateCourseOut)> GetExamples() =>
    [
        ("Exemplo", new() { Id = 1 }),
    ];
}
