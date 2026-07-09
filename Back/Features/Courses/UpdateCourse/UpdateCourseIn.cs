namespace Estud.Back.Features.Courses.UpdateCourse;

public class UpdateCourseIn : IApiDto<UpdateCourseIn>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public CourseType? Type { get; set; }

    public static IEnumerable<(string, UpdateCourseIn)> GetExamples() =>
    [
        ("Exemplo", new() { Id = 1, Name = "Análise e Desenvolvimento de Sistemas", Type = CourseType.Tecnologo }),
    ];
}
