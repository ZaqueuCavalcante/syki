namespace Syki.Front.Features.Academic.StartClass;

public class StartClassClient(HttpClient http)
{
    public async Task<HttpResponseMessage> Start(Guid id)
    {
        return await http.PutAsJsonAsync($"/academic/classes/{id}/start", new { });
    }
}
