using Exato.Shared.Auth;

namespace Exato.Shared.Features.Office.GetPolicies;

public class GetPoliciesOut : IApiDto<GetPoliciesOut>
{
    public List<GetPoliciesItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetPoliciesOut)> GetExamples() =>
    [
        ("Exemplo", new GetPoliciesOut() { }),
    ];
}

public class GetPoliciesItemOut
{
    public string Policy { get; set; }
    public List<ExatoFeature> Features { get; set; } = [];
}
