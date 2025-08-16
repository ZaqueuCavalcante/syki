namespace Syki.Front.Features.Academic.GetWebhookSubscription;

public class GetWebhookSubscriptionClient(HttpClient http) : IAcademicClient
{
    public async Task<GetWebhookSubscriptionOut> Get(Guid id)
    {
        return await http.GetFromJsonAsync<GetWebhookSubscriptionOut>($"/academic/webhooks/{id}", HttpConfigs.JsonOptions) ?? new();
    }
}
