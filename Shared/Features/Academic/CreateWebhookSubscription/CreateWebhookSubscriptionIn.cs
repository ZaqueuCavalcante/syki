namespace Syki.Shared;

public class CreateWebhookSubscriptionIn
{
    public string Name { get; set; }
    public string Url { get; set; }

    public List<WebhookEvent> Events { get; set; }

    public WebhookAuthenticationType AuthenticationType { get; set; }
    public string? ApiKey { get; set; }
}
