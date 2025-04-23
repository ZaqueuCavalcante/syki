using Audit.Core;
using System.Text.Json;
using Audit.EntityFramework;

namespace Syki.Back.Audit;

/// <summary>
/// Tabela de logs de auditoria.
/// </summary>
public class AuditLog
{
    public Guid Id { get; set; }
    public Guid EntityId { get; set; }
    public string EntityType { get; set; }
    public string Action { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid UserId { get; set; }
    public Guid InstitutionId { get; set; }
    public JsonDocument Data { get; set; }

    public bool Fill(AuditEvent evt, EventEntry entry)
    {
        if (evt.CustomFields.Count == 0 || evt.CustomFields.ContainsKey("Skip"))
            return false;

        Id = Guid.NewGuid();
        EntityId = Guid.Parse(entry.PrimaryKey.First().Value.ToString()!);
        EntityType = entry.EntityType.Name;
        Action = entry.Action;
        CreatedAt = DateTime.UtcNow;
        UserId = Guid.Parse(evt.CustomFields["UserId"].ToString()!);
        InstitutionId = Guid.Parse(evt.CustomFields["InstitutionId"].ToString()!);
        Data = AuditData.NewAsJson(entry);

        return true;
    }
}
