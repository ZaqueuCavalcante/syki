namespace Syki.Back.Features.Courses.UpdateCourse;

public class UpdateCourseOut : IApiDto<UpdateCourseOut>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public CourseType Type { get; set; }

    public static IEnumerable<(string, UpdateCourseOut)> GetExamples() =>
    [
        ("Exemplo", new() { Id = 1, Name = "Análise e Desenvolvimento de Sistemas", Type = CourseType.Tecnologo }),
    ];
}
