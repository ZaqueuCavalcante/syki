namespace Syki.Front.Features.Academic.GetWebhook;

public class GetWebhookClient(HttpClient http) : IAcademicClient
{
    public async Task<GetWebhookOut> Get(Guid id)
    {
        return await http.GetFromJsonAsync<GetWebhookOut>($"/academic/webhooks/{id}", HttpConfigs.JsonOptions) ?? new();
    }
}
