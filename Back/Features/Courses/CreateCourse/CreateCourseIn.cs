namespace Estud.Back.Features.Courses.CreateCourse;

public class CreateCourseIn : IApiDto<CreateCourseIn>
{
    public string Name { get; set; }
    public CourseType? Type { get; set; }

    public static IEnumerable<(string, CreateCourseIn)> GetExamples() =>
    [
        ("Exemplo", new() { Name = "Análise e Desenvolvimento de Sistemas", Type = CourseType.Tecnologo }),
    ];
}
