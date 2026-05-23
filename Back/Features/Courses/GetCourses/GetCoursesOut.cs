namespace Syki.Back.Features.Courses.GetCourses;

public class GetCoursesOut : IApiDto<GetCoursesOut>
{
    public int Total { get; set; }
    public List<GetCoursesItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetCoursesOut)> GetExamples() =>
    [
        ("Exemplo", new() { Total = 1, Items = [new() { Id = 1, Name = "Análise e Desenvolvimento de Sistemas", Type = "Tecnólogo" }] }),
    ];
}

public class GetCoursesItemOut
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public CourseType TypeValue { get; set; }
}
