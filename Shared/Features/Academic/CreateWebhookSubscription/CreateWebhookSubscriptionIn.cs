namespace Syki.Shared;

public class CreateWebhookSubscriptionIn
{
    /// <summary>
    /// Nome descritivo para o Webhook.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Url do Webhook.
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// Lista de eventos que serão enviados para o Webhook.
    /// </summary>
    public List<WebhookEventType> Events { get; set; }

    /// <summary>
    /// Tipo de autenticação do endpoint.
    /// </summary>
    public WebhookAuthenticationType AuthenticationType { get; set; }

    /// <summary>
    /// Chave de Api para autenticação no endpoint.
    /// </summary>
    public string? ApiKey { get; set; }
}
