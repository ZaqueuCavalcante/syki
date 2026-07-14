namespace Estud.Back.Features.Disciplines.GetDisciplines;

public class GetDisciplinesIn : IApiDto<GetDisciplinesIn>
{
    public string? Filter { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public static IEnumerable<(string, GetDisciplinesIn)> GetExamples() =>
    [
        ("Exemplo", new() { }),
    ];
}
