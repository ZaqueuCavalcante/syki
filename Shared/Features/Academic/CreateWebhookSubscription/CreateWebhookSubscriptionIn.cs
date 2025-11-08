namespace Syki.Shared;

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

    /// <summary>
    /// Tipo de autenticação do endpoint.
    /// </summary>
    public WebhookAuthenticationType AuthenticationType { get; set; }

    /// <summary>
    /// Chave de Api para autenticação no endpoint.
    /// </summary>
    public string? ApiKey { get; set; }

    public static IEnumerable<(string, CreateWebhookSubscriptionIn)> GetExamples() =>
    [
        ("Aluno criado",
        new CreateWebhookSubscriptionIn
        {
            Name = "Aluno criado",
            Url = "https://webhook.site/my-webhook",
            AuthenticationType = WebhookAuthenticationType.ApiKey,
            ApiKey = "z3Q6uDUJYTDCIo16myBKZrlCS63IvpCUOAE5X",
            Events = [WebhookEventType.StudentCreated]
        }),
        ("Atividade publicada",
        new CreateWebhookSubscriptionIn
        {
            Name = "Atividade publicada",
            Url = "https://webhook.site/my-other-webhook",
            AuthenticationType = WebhookAuthenticationType.ApiKey,
            ApiKey = "z3Q6uDUJYTDCIo16myBKZrlCS63IvpCUOAE5X",
            Events = [WebhookEventType.ClassActivityCreated]
        }),
    ];
}
