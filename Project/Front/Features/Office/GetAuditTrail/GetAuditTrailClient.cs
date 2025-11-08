using Exato.Shared.Features.Office.GetAuditTrail;

namespace Exato.Front.Features.Office.GetAuditTrail;

public class GetAuditTrailClient(HttpClient http) : IOfficeClient
{
    public async Task<OneOf<GetAuditTrailOut, ErrorOut>> Get(Guid id)
    {
        var response = await http.GetAsync($"office/audit-trails/{id}");

        return await response.Resolve<GetAuditTrailOut>();
    }
}
