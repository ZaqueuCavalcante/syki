namespace Syki.Back.Features.Academic.CallWebhooks;

/// <summary>
/// Chamada de Webhook
/// </summary>
public class WebhookCall : Entity
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public Guid WebhookId { get; set; }
    public string Payload { get; set; }
    public WebhookEventType Event { get; set; }
    public WebhookCallStatus Status { get; set; }
    public int AttemptsCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<WebhookCallAttempt> Attempts { get; set; }

    private WebhookCall() { }

    public WebhookCall(
        Guid institutionId,
        Guid webhookId,
        object data,
        DomainEventId eventId,
        WebhookEventType eventType)
    {
        Id = Guid.CreateVersion7();
        InstitutionId = institutionId;
        WebhookId = webhookId;
        Payload = (new { EventId = eventId, EventType = eventType, Data = data }).Serialize();
        Event = eventType;
        CreatedAt = DateTime.UtcNow;

        AddDomainEvent(new WebhookCallCreatedDomainEvent(Id));
    }

    public void Success(int statusCode, string response)
    {
        AttemptsCount++;
        Status = WebhookCallStatus.Success;
        Attempts.Add(new WebhookCallAttempt(Id, WebhookCallAttemptStatus.Success, statusCode, response));
    }

    public void Failed(int statusCode, string response)
    {
        Status = WebhookCallStatus.Error;
        Attempts.Add(new WebhookCallAttempt(Id, WebhookCallAttemptStatus.Error, statusCode, response));

        AttemptsCount++;

        if (AttemptsCount < 4)
        {
            var delaySeconds = AttemptsCount switch
            {
                1 => 1 * 60,
                2 => 5 * 60,
                3 => 30 * 60,
                _ => 0
            };

            AddDomainEvent(new WebhookCallFailedDomainEvent(Id, delaySeconds));
        }
    }

    public GetWebhookCallOut ToGetWebhookCallOut()
    {
        return new GetWebhookCallOut
        {
            Id = Id,
            Event = Event,
            Status = Status,
            CreatedAt = CreatedAt,
            AttemptsCount = AttemptsCount,
        };
    }


    public GetWebhookCallFullOut ToGetWebhookCallFullOut(string webhookName)
    {
        return new()
        {
            Id = Id,
            Event = Event,
            Status = Status,
            Payload = Payload,
            CreatedAt = CreatedAt,
            WebhookId = WebhookId,
            WebhookName = webhookName,
            AttemptsCount = AttemptsCount,
            Attempts = Attempts.OrderByDescending(x => x.CreatedAt).Select(x => x.ToOut()).ToList(),
        };
    }
}
