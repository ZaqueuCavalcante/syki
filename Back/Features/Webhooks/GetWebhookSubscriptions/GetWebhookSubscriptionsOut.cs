namespace Syki.Back.Features.Webhooks.GetWebhookSubscriptions;

public class GetWebhookSubscriptionsOut : IApiDto<GetWebhookSubscriptionsOut>
{
    public int Total { get; set; }
    public List<GetWebhookSubscriptionsItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetWebhookSubscriptionsOut)> GetExamples() =>
    [
        ("Exemplo",
        new GetWebhookSubscriptionsOut
        {
            Total = 1,
            Items =
            [
                new GetWebhookSubscriptionsItemOut
                {
                    Id = 1,
                    Name = "Aluno criado",
                    Url = "https://webhook.site/my-webhook",
                    IsActive = true,
                    Events = [WebhookEventType.StudentCreated],
                    CustomHeaders = new() { ["Exato-AuthToken"] = "6r4g654rs6g4we6f4qw684f68qwf4" },
                    CreatedAt = DateTime.UtcNow,
                },
            ],
        }),
    ];
}

public class GetWebhookSubscriptionsItemOut
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public bool IsActive { get; set; }
    public List<WebhookEventType> Events { get; set; }
    public Dictionary<string, string> CustomHeaders { get; set; } = [];
    public DateTime CreatedAt { get; set; }
}
