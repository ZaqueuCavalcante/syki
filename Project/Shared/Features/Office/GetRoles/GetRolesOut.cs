using Exato.Shared.Auth;

namespace Exato.Shared.Features.Office.GetRoles;

public class GetRolesOut : IApiDto<GetRolesOut>
{
    public int Total { get; set; }
    public List<GetRolesItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetRolesOut)> GetExamples() =>
    [
        ("Exemplo", new GetRolesOut() { }),
    ];
}

public class GetRolesItemOut
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int OrganizationId { get; set; }
    public string Organization { get; set; }
    public List<int> Features { get; set; } = [];

    public string GetFeaturesFraction()
    {
        return $"{Features.Count.ToIntFormat()}/{ExatoFeaturesStore.Features.Count.ToIntFormat()}";
    }
}
