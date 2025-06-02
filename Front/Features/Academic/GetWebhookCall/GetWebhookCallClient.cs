namespace Syki.Front.Features.Academic.GetWebhookCall;

public class GetWebhookCallClient(HttpClient http) : IAcademicClient
{
    public async Task<GetWebhookCallFullOut> Get(Guid id)
    {
        return await http.GetFromJsonAsync<GetWebhookCallFullOut>($"/academic/webhooks/calls/{id}", HttpConfigs.JsonOptions) ?? new();
    }
}
