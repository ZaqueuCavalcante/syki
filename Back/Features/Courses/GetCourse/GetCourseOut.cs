namespace Estud.Back.Features.Courses.GetCourse;

public class GetCourseOut : IApiDto<GetCourseOut>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public List<GetCourseDisciplineOut> Disciplines { get; set; } = [];

    public static IEnumerable<(string, GetCourseOut)> GetExamples() =>
    [
        ("Exemplo", new() { Id = 1, Name = "ADS", Type = "Tecnólogo", Disciplines = [new() { Id = 1, Name = "Cálculo I" }] }),
    ];
}

public class GetCourseDisciplineOut
{
    public int Id { get; set; }
    public string Name { get; set; }
}
