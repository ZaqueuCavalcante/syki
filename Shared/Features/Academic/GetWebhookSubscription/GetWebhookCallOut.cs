namespace Syki.Shared;

public class GetWebhookCallOut
{
    public Guid Id { get; set; }
    public WebhookEventType Event { get; set; }
    public WebhookCallStatus Status { get; set; }
    public int AttemptsCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
