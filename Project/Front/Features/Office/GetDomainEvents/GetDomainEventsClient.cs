using Exato.Shared.Features.Office.GetDomainEvents;

namespace Exato.Front.Features.Office.GetDomainEvents;

public class GetDomainEventsClient(HttpClient http) : IOfficeClient
{
    public async Task<GetDomainEventsOut> Get(GetDomainEventsIn data)
    {
        return await http.GetFromJsonAsync<GetDomainEventsOut>("office/domain-events".AddQueryString(data), HttpConfigs.JsonOptions) ?? new();
    }
}
