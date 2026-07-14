using System.Net.Http.Json;
using Estud.Back.Features.Webhooks.GetWebhookCalls;
using Estud.Back.Features.Webhooks.GetWebhookSubscription;
using Estud.Back.Features.Webhooks.GetWebhookSubscriptions;
using Estud.Back.Features.Webhooks.CreateWebhookSubscription;
using Estud.Back.Features.Webhooks.UpdateWebhookSubscription;

namespace Estud.Tests.Integration.Clients;

public partial class TestsHttpClient
{
    public async Task<OneOf<CreateWebhookSubscriptionOut, ErrorOut>> CreateWebhookSubscription(
        string name = "Aluno criado",
        string url = "https://webhook.site/my-webhook",
        List<WebhookEventType>? events = null,
        Dictionary<string, string>? customHeaders = null
    ) {
        var data = new CreateWebhookSubscriptionIn
        {
            Name = name,
            Url = url,
            Events = events ?? [WebhookEventType.StudentCreated],
            CustomHeaders = customHeaders ?? [],
        };
        var response = await http.PostAsJsonAsync("webhooks/subscriptions", data);
        return await response.Resolve<CreateWebhookSubscriptionOut>();
    }

    public async Task<OneOf<GetWebhookSubscriptionsOut, ErrorOut>> GetWebhookSubscriptions()
    {
        var response = await http.GetAsync("webhooks/subscriptions");
        return await response.Resolve<GetWebhookSubscriptionsOut>();
    }

    public async Task<OneOf<GetWebhookSubscriptionOut, ErrorOut>> GetWebhookSubscription(int id)
    {
        var response = await http.GetAsync($"webhooks/subscriptions/{id}");
        return await response.Resolve<GetWebhookSubscriptionOut>();
    }

    public async Task<OneOf<GetWebhookCallsOut, ErrorOut>> GetWebhookCalls(int page = 1, int pageSize = 20)
    {
        var data = new GetWebhookCallsIn
        {
            Page = page,
            PageSize = pageSize,
        };

        var response = await http.GetAsync("webhooks/calls".AddQueryString(data));
        return await response.Resolve<GetWebhookCallsOut>();
    }

    public async Task<OneOf<UpdateWebhookSubscriptionOut, ErrorOut>> UpdateWebhookSubscription(
        int id,
        string name = "Aluno criado",
        string url = "https://webhook.site/my-webhook",
        bool isActive = true,
        List<WebhookEventType>? events = null,
        Dictionary<string, string>? customHeaders = null
    ) {
        var data = new UpdateWebhookSubscriptionIn
        {
            Id = id,
            Name = name,
            Url = url,
            IsActive = isActive,
            Events = events ?? [WebhookEventType.StudentCreated],
            CustomHeaders = customHeaders ?? [],
        };
        var response = await http.PutAsJsonAsync("webhooks/subscriptions", data);
        return await response.Resolve<UpdateWebhookSubscriptionOut>();
    }
}
