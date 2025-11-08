using Exato.Shared.Auth;
using Exato.Shared.Features.Office.GetPolicies;

namespace Exato.Back.Features.Office.GetPolicies;

public class GetPoliciesService : IOfficeService
{
    public GetPoliciesOut Get()
    {
        var result = new GetPoliciesOut();

        var policies = new List<PolicyMetadata>();
        policies.AddRange(Policies.Cross);
        policies.AddRange(Policies.Office);

        foreach (var policy in policies)
        {
            var item = new GetPoliciesItemOut() { Policy = policy.Name };
            item.Features = policy.Features;
            result.Items.Add(item);
        }

        return result;
    }
}
