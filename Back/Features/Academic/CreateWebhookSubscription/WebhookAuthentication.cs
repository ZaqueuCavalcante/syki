namespace Syki.Back.Features.Academic.CreateWebhookSubscription;

/// <summary>
/// Inscrição de Webhook para receber notificações sobre eventos acadêmicos.
/// </summary>
public class WebhookAuthentication
{
    public Guid Id { get; set; }
    public Guid WebhookId { get; set; }
    public WebhookAuthenticationType Type { get; set; }
    public string ApiKey { get; set; }

    private WebhookAuthentication() { }

    public WebhookAuthentication(Guid webhookId, WebhookAuthenticationType type, string apiKey)
    {
        Id = Guid.NewGuid();
        WebhookId = webhookId;
        Type = type;
        ApiKey = apiKey;
    }
}
