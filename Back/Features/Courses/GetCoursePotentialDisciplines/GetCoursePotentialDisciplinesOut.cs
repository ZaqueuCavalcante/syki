namespace Syki.Back.Features.Courses.GetCoursePotentialDisciplines;

public class GetCoursePotentialDisciplinesOut : IApiDto<GetCoursePotentialDisciplinesOut>
{
    public List<GetCoursePotentialDisciplineItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetCoursePotentialDisciplinesOut)> GetExamples() =>
    [
        ("Exemplo", new() { Items = [new() { Id = 1, Name = "Cálculo I" }] }),
    ];
}

public class GetCoursePotentialDisciplineItemOut
{
    public int Id { get; set; }
    public string Name { get; set; }
}
