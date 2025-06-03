using Syki.Back.Features.Academic.CallWebhooks;

namespace Syki.Back.Features.Academic.CreateWebhookSubscription;

/// <summary>
/// Inscrição de Webhook para receber notificações sobre eventos acadêmicos.
/// </summary>
public class WebhookSubscription
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<WebhookEventType> Events { get; set; }
    public WebhookAuthentication Authentication { get; set; }

    public List<WebhookCall> Calls { get; set; }

    private WebhookSubscription() { }

    public WebhookSubscription(Guid institutionId, string name, string url, List<WebhookEventType> events, string apiKey)
    {
        Id = Guid.NewGuid();
        InstitutionId = institutionId;
        Name = name;
        Url = url;
        Events = events;
        CreatedAt = DateTime.UtcNow;
        Authentication = new WebhookAuthentication(Id, WebhookAuthenticationType.ApiKey, apiKey);
    }

    public CreateWebhookSubscriptionOut ToOut()
    {
        return new()
        {
            Id = Id,
        };
    }

    public GetWebhooksOut ToGetWebhooksOut()
    {
        return new()
        {
            Id = Id,
            Name = Name,
            Url = Url,
            CreatedAt = CreatedAt,
            CallsCount = Calls.Count,
        };
    }

    public GetWebhookOut ToGetWebhookOut()
    {
        return new()
        {
            Id = Id,
            Url = Url,
            Name = Name,
            CreatedAt = CreatedAt,
            CallsCount = Calls.Count,
            EventsCount = Events.Count,
            AuthenticationType = Authentication.Type,
            Calls = Calls.ConvertAll(x => x.ToGetWebhookCallOut())
        };
    }
}
