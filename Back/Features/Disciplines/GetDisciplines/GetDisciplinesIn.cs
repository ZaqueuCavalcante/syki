namespace Syki.Back.Features.Disciplines.GetDisciplines;

public class GetDisciplinesIn : IApiDto<GetDisciplinesIn>
{
    public string? Filter { get; set; }

    public static IEnumerable<(string, GetDisciplinesIn)> GetExamples() =>
    [
        ("Exemplo", new() { }),
    ];
}
