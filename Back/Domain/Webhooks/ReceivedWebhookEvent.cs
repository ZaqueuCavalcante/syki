using Syki.Back.Commands.Domain.Commands;

namespace Syki.Back.Domain.Webhooks;

public class ReceivedWebhookEvent
{
    public int Id { get; set; }
    public string ExternalId { get; set; }
    public ReceivedWebhookEventSource Source { get; set; }
    public string Type { get; set; }
    public string Payload { get; set; }
    public ReceivedWebhookEventStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public Guid? ProcessorId { get; set; }
    public string? Error { get; set; }
    public int? CommandId { get; set; }

    public Command? Command { get; set; }
}
