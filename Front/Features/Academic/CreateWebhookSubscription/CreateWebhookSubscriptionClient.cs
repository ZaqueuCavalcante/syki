namespace Syki.Front.Features.Academic.CreateWebhookSubscription;

public class CreateWebhookSubscriptionClient(HttpClient http) : IAcademicClient
{
    public async Task<OneOf<CreateWebhookSubscriptionOut, ErrorOut>> Create(
        string name,
        string url,
        List<WebhookEvent> events,
        WebhookAuthenticationType AuthenticationType,
        string? apiKey
    ) {
        var data = new CreateWebhookSubscriptionIn
        {
            Name = name,
            Url = url,
            Events = events,
            AuthenticationType = AuthenticationType,
            ApiKey = apiKey
        };

        var response = await http.PostAsJsonAsync("/academic/webhooks", data);

        return await response.Resolve<CreateWebhookSubscriptionOut>();
    }
}
