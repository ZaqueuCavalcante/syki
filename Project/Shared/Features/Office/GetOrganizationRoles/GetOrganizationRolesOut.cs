namespace Exato.Shared.Features.Office.GetOrganizationRoles;

public class GetOrganizationRolesOut : IApiDto<GetOrganizationRolesOut>
{
    public int Total { get; set; }
    public List<GetOrganizationRolesItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetOrganizationRolesOut)> GetExamples() =>
    [
        ("Exemplo", new GetOrganizationRolesOut() { }),
    ];
}

public class GetOrganizationRolesItemOut
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
