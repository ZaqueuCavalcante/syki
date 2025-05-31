namespace Syki.Front.Features.Academic.GetWebhooks;

public class GetWebhooksClient(HttpClient http) : IAcademicClient
{
    public async Task<List<GetWebhooksOut>> Get()
    {
        return await http.GetFromJsonAsync<List<GetWebhooksOut>>("/academic/webhooks", HttpConfigs.JsonOptions) ?? [];
    }
}
