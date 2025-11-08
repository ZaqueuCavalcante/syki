namespace Exato.Shared.Features.Office.GetUsers;

public class GetUsersIn : IApiDto<GetUsersIn>
{
    public int Page { get; set; }
    public string? Name { get; set; }

    public static IEnumerable<(string, GetUsersIn)> GetExamples() =>
    [
        ("Exemplo", new GetUsersIn() { }),
    ];
}
