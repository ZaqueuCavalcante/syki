namespace Estud.Back.Features.Webhooks.GetWebhookSubscription;

public class GetWebhookSubscriptionOut : IApiDto<GetWebhookSubscriptionOut>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public bool IsActive { get; set; }
    public List<WebhookEventType> Events { get; set; }
    public Dictionary<string, string> CustomHeaders { get; set; } = [];
    public DateTime CreatedAt { get; set; }

    public static IEnumerable<(string, GetWebhookSubscriptionOut)> GetExamples() =>
    [
        ("Exemplo",
        new GetWebhookSubscriptionOut
        {
            Id = 1,
            Name = "Aluno criado",
            Url = "https://webhook.site/my-webhook",
            IsActive = true,
            Events = [WebhookEventType.StudentCreated],
            CustomHeaders = new() { ["Estud-AuthToken"] = "6r4g654rs6g4we6f4qw684f68qwf4" },
            CreatedAt = DateTime.UtcNow,
        }),
    ];
}
