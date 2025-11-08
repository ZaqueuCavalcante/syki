using Audit.Core;
using System.Text.Json;
using Audit.EntityFramework;

namespace Exato.Back.Audit;

/// <summary>
/// Guarda os dados de auditoria de uma entidade.
/// </summary>
public class AuditTrail
{
    public Guid Id { get; set; }

    public string ActivityId { get; set; }
    public string Operation { get; set; }

    public string EntityId { get; set; }
    public string EntityType { get; set; }

    public Guid UserId { get; set; }
    public int OrganizationId { get; set; }

    public string Action { get; set; }
    public DateTime CreatedAt { get; set; }
    public JsonDocument Data { get; set; }

    public bool Fill(AuditEvent evt, EventEntry entry)
    {
        Id = Guid.CreateVersion7();

        ActivityId = evt.ActivityId;
        Operation = evt.Operation;

        EntityId = entry.PrimaryKey.First().Value.ToString()!;
        EntityType = entry.EntityType.Name;

        UserId = evt.UserId;
        OrganizationId = evt.OrganizationId;

        Action = entry.Action;
        CreatedAt = DateTime.Now;
        Data = AuditData.NewAsJson(entry);

        return true;
    }
}
