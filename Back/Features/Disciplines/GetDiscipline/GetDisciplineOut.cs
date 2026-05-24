namespace Syki.Back.Features.Disciplines.GetDiscipline;

public class GetDisciplineOut : IApiDto<GetDisciplineOut>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public List<GetDisciplineCourseOut> Courses { get; set; } = [];

    public static IEnumerable<(string, GetDisciplineOut)> GetExamples() =>
    [
        ("Exemplo", new() { Id = 1, Name = "Cálculo I", Code = "ABC12345", Courses = [new() { Id = 1, Name = "ADS" }] }),
    ];
}

public class GetDisciplineCourseOut
{
    public int Id { get; set; }
    public string Name { get; set; }
}
