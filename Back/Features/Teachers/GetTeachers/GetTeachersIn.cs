namespace Estud.Back.Features.Teachers.GetTeachers;

public class GetTeachersIn : IApiDto<GetTeachersIn>
{
    public string? Filter { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public static IEnumerable<(string, GetTeachersIn)> GetExamples() =>
    [
        ("Exemplo", new() { }),
    ];
}
