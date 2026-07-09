namespace Estud.Back.Features.Webhooks.CreateWebhookSubscription;

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
    /// Headers customizados (pares chave-valor) enviados em todas as chamadas feitas para a Url do Webhook.
    /// Útil, por exemplo, para autenticação via header.
    /// </summary>
    public Dictionary<string, string> CustomHeaders { get; set; } = [];

    public static IEnumerable<(string, CreateWebhookSubscriptionIn)> GetExamples() =>
    [
        ("Aluno criado",
        new CreateWebhookSubscriptionIn
        {
            Name = "Aluno criado",
            Url = "https://webhook.site/my-webhook",
            Events = [WebhookEventType.StudentCreated],
            CustomHeaders = new() { ["Estud-AuthToken"] = "6r4g654rs6g4we6f4qw684f68qwf4" }
        }),
        ("Atividade publicada",
        new CreateWebhookSubscriptionIn
        {
            Name = "Atividade publicada",
            Url = "https://webhook.site/my-other-webhook",
            Events = [WebhookEventType.ClassActivityCreated],
            CustomHeaders = []
        }),
    ];
}
