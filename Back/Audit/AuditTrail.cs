using Audit.Core;
using System.Text.Json;
using Audit.EntityFramework;

namespace Estud.Back.Audit;

/// <summary>
/// Guarda os dados de auditoria de uma entidade.
/// </summary>
public class AuditTrail
{
    public int Id { get; set; }

    public string ActivityId { get; set; }
    public string Operation { get; set; }

    public string EntityId { get; set; }
    public string EntityType { get; set; }

    public int UserId { get; set; }
    public int InstitutionId { get; set; }

    public string Action { get; set; }
    public DateTime CreatedAt { get; set; }
    public JsonDocument Data { get; set; }

    public bool Fill(AuditEvent evt, EventEntry entry)
    {
        ActivityId = evt.ActivityId;
        Operation = evt.Operation;

        EntityId = entry.PrimaryKey.First().Value.ToString()!;
        EntityType = entry.EntityType.Name;

        UserId = evt.UserId;
        InstitutionId = evt.InstitutionId;

        Action = entry.Action;
        CreatedAt = DateTime.UtcNow;
        Data = AuditData.NewAsJson(entry);

        return true;
    }
}
