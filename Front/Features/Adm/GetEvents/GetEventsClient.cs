namespace Syki.Front.Features.Adm.GetEvents;

public class GetEventsClient(HttpClient http) : IAdmClient
{
    public async Task<GetEventsOut> Get()
    {
        return await http.GetFromJsonAsync<GetEventsOut>("/adm/events") ?? new();
    }
}
