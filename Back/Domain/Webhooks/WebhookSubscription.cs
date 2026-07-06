namespace Syki.Back.Domain.Webhooks;

/// <summary>
/// Inscrição de Webhook para receber notificações sobre eventos acadêmicos.
/// </summary>
public class WebhookSubscription
{
    public int Id { get; set; }
    public int InstitutionId { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<WebhookEventType> Events { get; set; }
    public List<WebhookCall> Calls { get; set; }

    /// <summary>
    /// Headers customizados enviados em todas as chamadas feitas para a Url do Webhook.
    /// </summary>
    public Dictionary<string, string> CustomHeaders { get; set; } = [];

    private WebhookSubscription() { }

    public WebhookSubscription(int institutionId, string name, string url, List<WebhookEventType> events, Dictionary<string, string> customHeaders)
    {
        InstitutionId = institutionId;
        Name = name;
        Url = url;
        Events = events;
        CustomHeaders = customHeaders ?? [];
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string url, List<WebhookEventType> events, Dictionary<string, string> customHeaders, bool isActive)
    {
        Name = name;
        Url = url;
        Events = events;
        CustomHeaders = customHeaders ?? [];
        IsActive = isActive;
    }
}
