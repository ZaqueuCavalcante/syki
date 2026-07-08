namespace Syki.Back.Features.Webhooks.GetWebhookCalls;

public class GetWebhookCallsOut : IApiDto<GetWebhookCallsOut>
{
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public List<GetWebhookCallsItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetWebhookCallsOut)> GetExamples() =>
    [
        ("Exemplo", new GetWebhookCallsOut
        {
            Total = 1,
            Page = 1,
            PageSize = 20,
            Items =
            [
                new GetWebhookCallsItemOut
                {
                    Id = 1,
                    EventType = WebhookEventType.StudentCreated,
                    Status = WebhookCallStatus.Success,
                    AttemptsCount = 1,
                    CreatedAt = DateTime.UtcNow,
                },
            ],
        }),
    ];
}

public class GetWebhookCallsItemOut
{
    public int Id { get; set; }
    public WebhookEventType EventType { get; set; }
    public WebhookCallStatus Status { get; set; }
    public int AttemptsCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
