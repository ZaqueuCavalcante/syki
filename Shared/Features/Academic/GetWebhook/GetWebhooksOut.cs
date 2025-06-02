namespace Syki.Shared;

public class GetWebhookOut
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public WebhookAuthenticationType AuthenticationType { get; set; }
    public DateTime CreatedAt { get; set; }
    public int EventsCount { get; set; }
    public int CallsCount { get; set; }

    public List<GetWebhookCallOut> Calls { get; set; }
}
