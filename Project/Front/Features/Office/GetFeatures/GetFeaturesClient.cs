using Exato.Shared.Features.Office.GetFeatures;

namespace Exato.Front.Features.Office.GetFeatures;

public class GetFeaturesClient(HttpClient http) : IOfficeClient
{
    public async Task<GetFeaturesOut> Get()
    {
        return await http.GetFromJsonAsync<GetFeaturesOut>("office/features", HttpConfigs.JsonOptions) ?? new();
    }
}
