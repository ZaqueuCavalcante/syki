using Exato.Shared.Features.Office.GetDomainEvent;

namespace Exato.Front.Features.Office.GetDomainEvent;

public class GetDomainEventClient(HttpClient http) : IOfficeClient
{
    public async Task<OneOf<GetDomainEventOut, ErrorOut>> Get(Guid id)
    {
        var response = await http.GetAsync($"office/domain-events/{id}");

        return await response.Resolve<GetDomainEventOut>();
    }
}
