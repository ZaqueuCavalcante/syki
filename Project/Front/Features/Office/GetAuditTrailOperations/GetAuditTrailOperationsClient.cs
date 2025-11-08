using Exato.Shared.Features.Office.GetAuditTrailOperations;

namespace Exato.Front.Features.Office.GetAuditTrailOperations;

public class GetAuditTrailOperationsClient(HttpClient http) : IOfficeClient
{
    public async Task<GetAuditTrailOperationsOut> Get()
    {
        return await http.GetFromJsonAsync<GetAuditTrailOperationsOut>("office/audit-trails/operations", HttpConfigs.JsonOptions) ?? new();
    }
}
