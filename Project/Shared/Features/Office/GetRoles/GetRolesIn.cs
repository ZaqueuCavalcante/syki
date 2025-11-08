namespace Exato.Shared.Features.Office.GetRoles;

public class GetRolesIn : IApiDto<GetRolesIn>
{
    public int Page { get; set; }
    public string? Name { get; set; }

    public static IEnumerable<(string, GetRolesIn)> GetExamples() =>
    [
        ("Exemplo", new GetRolesIn() { }),
    ];
}
