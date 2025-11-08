using Exato.Shared.Auth;

namespace Exato.Shared.Features.Office.GetFeatures;

public class GetFeaturesOut : IApiDto<GetFeaturesOut>
{
    public List<GetFeaturesItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetFeaturesOut)> GetExamples() =>
    [
        ("Exemplo", new GetFeaturesOut() { }),
    ];
}

public class GetFeaturesItemOut
{
    public ExatoFeature Feature { get; set; }
    public List<string> Policies { get; set; } = [];
}
