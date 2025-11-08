using Exato.Shared.Features.Office.GetPolicies;

namespace Exato.Front.Features.Office.GetPolicies;

public class GetPoliciesClient(HttpClient http) : IOfficeClient
{
    public async Task<GetPoliciesOut> Get()
    {
        return await http.GetFromJsonAsync<GetPoliciesOut>("office/policies", HttpConfigs.JsonOptions) ?? new();
    }
}
