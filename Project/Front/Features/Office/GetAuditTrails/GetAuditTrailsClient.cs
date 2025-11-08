using Exato.Shared.Features.Office.GetAuditTrails;

namespace Exato.Front.Features.Office.GetAuditTrails;

public class GetAuditTrailsClient(HttpClient http) : IOfficeClient
{
    public async Task<GetAuditTrailsOut> Get(GetAuditTrailsIn query)
    {
        return await http.GetFromJsonAsync<GetAuditTrailsOut>("office/audit-trails".AddQueryString(query), HttpConfigs.JsonOptions) ?? new();
    }
}
