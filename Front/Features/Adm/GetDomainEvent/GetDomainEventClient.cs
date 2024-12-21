namespace Syki.Front.Features.Adm.GetDomainEvent;

public class GetDomainEventClient(HttpClient http) : IAdmClient
{
    public async Task<DomainEventOut> Get(Guid id)
    {
        return await http.GetFromJsonAsync<DomainEventOut>($"/adm/domain-events/{id}") ?? new();
    }
}
