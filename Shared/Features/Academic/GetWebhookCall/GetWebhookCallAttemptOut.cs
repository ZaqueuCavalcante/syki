namespace Syki.Shared;

public class GetWebhookCallAttemptOut
{
    public WebhookCallAttemptStatus Status { get; set; }
    public int StatusCode { get; set; }
    public string Response { get; set; }
    public DateTime CreatedAt { get; set; }
}
