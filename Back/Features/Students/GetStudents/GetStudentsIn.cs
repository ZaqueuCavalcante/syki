namespace Estud.Back.Features.Students.GetStudents;

public class GetStudentsIn : IApiDto<GetStudentsIn>
{
    public string? Filter { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public static IEnumerable<(string, GetStudentsIn)> GetExamples() =>
    [
        ("Exemplo", new() { }),
    ];
}
