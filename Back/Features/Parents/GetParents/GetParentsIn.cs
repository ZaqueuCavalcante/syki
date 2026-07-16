namespace Estud.Back.Features.Parents.GetParents;

public class GetParentsIn : IApiDto<GetParentsIn>
{
    public string? Filter { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public static IEnumerable<(string, GetParentsIn)> GetExamples() =>
    [
        ("Exemplo", new() { }),
    ];
}
