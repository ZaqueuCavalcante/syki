using System.Text.Json;

namespace Syki.Back.Domain;

public class AuditLog
{
    public Guid Id { get; set; }
    public Guid EntityId { get; set; }
    public string EntityType { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid UserId { get; set; }
    public Guid FaculdadeId { get; set; }
    public JsonDocument Data { get; set; }
}
