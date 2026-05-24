namespace Syki.Back.Features.Disciplines.GetDisciplinePotentialCourses;

public class GetDisciplinePotentialCoursesOut : IApiDto<GetDisciplinePotentialCoursesOut>
{
    public List<GetDisciplinePotentialCourseItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetDisciplinePotentialCoursesOut)> GetExamples() =>
    [
        ("Exemplo", new() { Items = [new() { Id = 1, Name = "ADS" }] }),
    ];
}

public class GetDisciplinePotentialCourseItemOut
{
    public int Id { get; set; }
    public string Name { get; set; }
}
