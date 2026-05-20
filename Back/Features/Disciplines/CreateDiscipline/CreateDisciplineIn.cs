namespace Syki.Back.Features.Disciplines.CreateDiscipline;

public class CreateDisciplineIn : IApiDto<CreateDisciplineIn>
{
    public string Name { get; set; }
    public List<int> Courses { get; set; } = [];

    public static IEnumerable<(string, CreateDisciplineIn)> GetExamples() =>
    [
        ("Exemplo", new() { }),
    ];
}
