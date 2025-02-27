namespace Syki.Front.Features.Adm.GetDomainEvents;

public class GetDomainEventsClient(HttpClient http) : IAdmClient
{
    public async Task<List<DomainEventTableOut>> Get(DomainEventTableFilterIn filters)
    {
        return await http.GetFromJsonAsync<List<DomainEventTableOut>>("/adm/domain-events".AddQueryString(filters), HttpConfigs.JsonOptions) ?? [];
    }
}
