namespace Syki.Shared;

public class GetWebhookCallFullOut
{
    public Guid Id { get; set; }
    public Guid WebhookId { get; set; }
    public string WebhookName { get; set; }
    public WebhookEventType Event { get; set; }
    public WebhookCallStatus Status { get; set; }
    public string Payload { get; set; }
    public int AttemptsCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<GetWebhookCallAttemptOut> Attempts { get; set; } = [];
}
