using Audit.Core;
using System.Text.Json;
using Audit.EntityFramework;

namespace Syki.Back.Audit;

public class AuditLog
{
    public Guid Id { get; set; }
    public Guid EntityId { get; set; }
    public string EntityType { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid UserId { get; set; }
    public Guid FaculdadeId { get; set; }
    public JsonDocument Data { get; set; }

    public void Fill(AuditEvent evt, EventEntry entry)
    {
        Id = Guid.NewGuid();
        EntityId = Guid.Parse(entry.PrimaryKey.First().Value.ToString()!);
        EntityType = entry.EntityType.Name;
        CreatedAt = DateTime.Now;
        UserId = Guid.Parse(evt.CustomFields["UserId"].ToString()!);
        FaculdadeId = Guid.Parse(evt.CustomFields["FaculdadeId"].ToString()!);
        Data = AuditData.NewAsJson(entry);
    }
}
