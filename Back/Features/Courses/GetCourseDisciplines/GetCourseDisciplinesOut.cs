namespace Syki.Back.Features.Courses.GetCourseDisciplines;

public class GetCourseDisciplinesOut : IApiDto<GetCourseDisciplinesOut>
{
    public int Total { get; set; }
    public List<GetCourseDisciplineItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetCourseDisciplinesOut)> GetExamples() =>
    [
        ("Exemplo", new() { Total = 1, Items = [new() { Id = 1, Name = "Geometria" }] }),
    ];
}

public class GetCourseDisciplineItemOut
{
    public int Id { get; set; }
    public string Name { get; set; }
}
