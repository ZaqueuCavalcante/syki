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

    public List<WebhookEvent> Events { get; set; }
    public WebhookAuthentication Authentication { get; set; }

    private WebhookSubscription() { }

    public WebhookSubscription(Guid institutionId, string name, string url, List<WebhookEvent> events, string apiKey)
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
}
