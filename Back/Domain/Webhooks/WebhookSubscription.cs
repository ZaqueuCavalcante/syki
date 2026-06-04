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

    private WebhookSubscription() { }

    public WebhookSubscription(int institutionId, string name, string url, List<WebhookEventType> events)
    {
        InstitutionId = institutionId;
        Name = name;
        Url = url;
        Events = events;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string url, List<WebhookEventType> events, bool isActive)
    {
        Name = name;
        Url = url;
        Events = events;
        IsActive = isActive;
    }
}
