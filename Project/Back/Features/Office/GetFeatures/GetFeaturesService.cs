using Exato.Shared.Auth;
using Exato.Shared.Features.Office.GetFeatures;

namespace Exato.Back.Features.Office.GetFeatures;

public class GetFeaturesService : IOfficeService
{
    public GetFeaturesOut Get()
    {
        var result = new GetFeaturesOut();

        var policies = new List<PolicyMetadata>();
        policies.AddRange(Policies.Cross);
        policies.AddRange(Policies.Office);

        foreach (var feature in ExatoFeaturesStore.Features)
        {
            var item = new GetFeaturesItemOut() { Feature = feature };
            item.Policies = policies.Where(x => x.Features.Any(f => f.Id == feature.Id)).Select(x => x.Name).ToList();
            result.Items.Add(item);
        }

        return result;
    }
}
