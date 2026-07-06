namespace Syki.Back.Features.Webhooks.UpdateWebhookSubscription;

public class UpdateWebhookSubscriptionIn : IApiDto<UpdateWebhookSubscriptionIn>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public bool IsActive { get; set; }
    public List<WebhookEventType> Events { get; set; }

    /// <summary>
    /// Headers customizados (pares chave-valor) enviados em todas as chamadas feitas para a Url do Webhook.
    /// Útil, por exemplo, para autenticação via header.
    /// </summary>
    public Dictionary<string, string> CustomHeaders { get; set; } = [];

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
            CustomHeaders = new() { ["Exato-AuthToken"] = "6r4g654rs6g4we6f4qw684f68qwf4" },
        }),
    ];
}
