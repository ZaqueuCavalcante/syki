namespace Syki.Front.Features.Adm.GetDomainEventsSummary;

public class GetDomainEventsSummaryClient(HttpClient http) : IAdmClient
{
    public async Task<GetDomainEventsSummaryOut> Get()
    {
        return await http.GetFromJsonAsync<GetDomainEventsSummaryOut>("/adm/domain-events/summary", HttpConfigs.JsonOptions) ?? new();
    }
}
