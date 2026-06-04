namespace Syki.Back.Features.Webhooks.UpdateWebhookSubscription;

public class UpdateWebhookSubscriptionIn : IApiDto<UpdateWebhookSubscriptionIn>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public bool IsActive { get; set; }
    public List<WebhookEventType> Events { get; set; }

    public static IEnumerable<(string, UpdateWebhookSubscriptionIn)> GetExamples() =>
    [
        ("Exemplo",
        new UpdateWebhookSubscriptionIn
        {
            Id = 1,
            Name = "Aluno criado",
            Url = "https://webhook.site/my-webhook",
            IsActive = true,
            Events = [WebhookEventType.StudentCreated],
        }),
    ];
}
