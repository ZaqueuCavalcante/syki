namespace Exato.Shared.Features.Office.GetOrganizationRoles;

public class GetOrganizationRolesIn : IApiDto<GetOrganizationRolesIn>
{
    public int Page { get; set; }
    public string? Name { get; set; }

    public static IEnumerable<(string, GetOrganizationRolesIn)> GetExamples() =>
    [
        ("Exemplo", new GetOrganizationRolesIn() { }),
    ];
}
