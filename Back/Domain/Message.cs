using System.Text.Json;
using Syki.Back.Configs;

namespace Syki.Back.Domain;

public class Message
{
    public Guid Id { get; set; }
    public Guid EntityId { get; set; }
    public string Type { get; set; }
    public JsonDocument Data { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string? Error { get; set; }

    public Message() { }

    public Message(
        Guid entityId,
        object data
    ) {
        Id = Guid.NewGuid();
        EntityId = entityId;
        Type = data.GetType().ToString();
        Data = JsonDocument.Parse(data.Serialize());
        CreatedAt = DateTime.Now;
    }
}
