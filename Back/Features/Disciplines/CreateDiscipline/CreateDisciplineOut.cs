namespace Syki.Back.Features.Disciplines.CreateDiscipline;

public class CreateDisciplineOut : IApiDto<CreateDisciplineOut>
{
    public int Id { get; set; }

    public static IEnumerable<(string, CreateDisciplineOut)> GetExamples() =>
    [
        ("Exemplo", new() { }),
    ];
}
