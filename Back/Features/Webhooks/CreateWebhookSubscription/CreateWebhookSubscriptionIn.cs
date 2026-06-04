namespace Syki.Back.Features.Webhooks.CreateWebhookSubscription;

public class CreateWebhookSubscriptionIn : IApiDto<CreateWebhookSubscriptionIn>
{
    /// <summary>
    /// Nome descritivo para o Webhook.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Url do Webhook.
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// Lista de eventos que serão enviados para o Webhook.
    /// </summary>
    public List<WebhookEventType> Events { get; set; }

    public static IEnumerable<(string, CreateWebhookSubscriptionIn)> GetExamples() =>
    [
        ("Aluno criado",
        new CreateWebhookSubscriptionIn
        {
            Name = "Aluno criado",
            Url = "https://webhook.site/my-webhook",
            Events = [WebhookEventType.StudentCreated]
        }),
        ("Atividade publicada",
        new CreateWebhookSubscriptionIn
        {
            Name = "Atividade publicada",
            Url = "https://webhook.site/my-other-webhook",
            Events = [WebhookEventType.ClassActivityCreated]
        }),
    ];
}
