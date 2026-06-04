namespace Syki.Back.Domain.Webhooks;

/// <summary>
/// Tentativa de chamada de Webhook
/// </summary>
public class WebhookCallAttempt
{
    public int Id { get; set; }
    public int WebhookCallId { get; set; }
    public WebhookCallAttemptStatus Status { get; set; }
    public int StatusCode { get; set; }
    public string Response { get; set; }
    public DateTime CreatedAt { get; set; }

    private WebhookCallAttempt() { }

    public WebhookCallAttempt(int webhookCallId, WebhookCallAttemptStatus status, int statusCode, string response)
    {
        WebhookCallId = webhookCallId;
        Status = status;
        StatusCode = statusCode;
        Response = response;
        CreatedAt = DateTime.UtcNow;
    }
}
