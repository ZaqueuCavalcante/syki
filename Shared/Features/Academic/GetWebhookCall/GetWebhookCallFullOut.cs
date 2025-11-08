namespace Syki.Shared;

public class GetWebhookCallFullOut : IApiDto<GetWebhookCallFullOut>
{
    public Guid Id { get; set; }
    public Guid WebhookId { get; set; }
    public string WebhookName { get; set; }
    public WebhookEventType Event { get; set; }
    public WebhookCallStatus Status { get; set; }
    public string Payload { get; set; }
    public int AttemptsCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<GetWebhookCallAttemptOut> Attempts { get; set; } = [];

    public static IEnumerable<(string, GetWebhookCallFullOut)> GetExamples() =>
    [
        (
            "Success",
            new GetWebhookCallFullOut
            {
                Id = Guid.CreateVersion7(),
                WebhookId = Guid.CreateVersion7(),
                WebhookName = "GitHub",
                Event = WebhookEventType.StudentCreated,
                Status = WebhookCallStatus.Success,
                Payload = """{"UserId":"01983c1f-d716-75a9-ac70-12ab881415c4","InstitutionId":"01983c1f-7489-7b95-9d5a-e9f5d9568a47"}""",
                AttemptsCount = 1,
                CreatedAt = DateTime.UtcNow.AddMinutes(-30),
                Attempts =
                [
                    new()
                    {
                        Status = WebhookCallAttemptStatus.Success,
                        StatusCode = 200,
                        Response = "",
                        CreatedAt = DateTime.UtcNow.AddMinutes(-29),
                    }
                ]
            }
        ),
        (
            "Error",
            new GetWebhookCallFullOut
            {
                Id = Guid.CreateVersion7(),
                WebhookId = Guid.CreateVersion7(),
                WebhookName = "GitHub",
                Event = WebhookEventType.StudentCreated,
                Status = WebhookCallStatus.Error,
                Payload = """{"UserId":"01983c1f-d716-75a9-ac70-12ab881415c4","InstitutionId":"01983c1f-7489-7b95-9d5a-e9f5d9568a47"}""",
                AttemptsCount = 2,
                CreatedAt = DateTime.UtcNow.AddMinutes(-30),
                Attempts =
                [
                    new()
                    {
                        Status = WebhookCallAttemptStatus.Error,
                        StatusCode = 400,
                        Response = "User Not Found",
                        CreatedAt = DateTime.UtcNow.AddMinutes(-29),
                    },
                    new()
                    {
                        Status = WebhookCallAttemptStatus.Error,
                        StatusCode = 400,
                        Response = "User Not Found",
                        CreatedAt = DateTime.UtcNow.AddMinutes(-25),
                    }
                ]
            }
        )
    ];
}
