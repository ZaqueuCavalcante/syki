namespace Syki.Back.Features.Academic.CallWebhooks;

/// <summary>
/// Tentativa de chamada de Webhook
/// </summary>
public class WebhookCallAttempt
{
    public Guid Id { get; set; }
    public Guid WebhookCallId { get; set; }
    public WebhookCallAttemptStatus Status { get; set; }
    public int StatusCode { get; set; }
    public string Response { get; set; }
    public DateTime CreatedAt { get; set; }

    private WebhookCallAttempt() { }

    public WebhookCallAttempt(Guid webhookCallId, WebhookCallAttemptStatus status, int statusCode, string response)
    {
        Id = Guid.NewGuid();
        WebhookCallId = webhookCallId;
        Status = status;
        StatusCode = statusCode;
        Response = response;
        CreatedAt = DateTime.UtcNow;
    }
}
