namespace Estud.Back.Features.Disciplines.GetDisciplines;

public class GetDisciplinesOut : IApiDto<GetDisciplinesOut>
{
    public int Total { get; set; }
    public List<GetDisciplinesItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetDisciplinesOut)> GetExamples() =>
    [
        ("Exemplo", new() { }),
    ];
}

public class GetDisciplinesItemOut
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public int Courses { get; set; }
    public int Teachers { get; set; }
}
