using System.Text.Json;
using Audit.EntityFramework;

namespace Estud.Back.Audit;

/// <summary>
/// Guarda as informações da autoria de mudança de uma entidade.
/// </summary>
public class AuditData
{
    public string Name { get; set; }
    public string Table { get; set; }
    public string Schema { get; set; }
    public List<AuditChange> Changes { get; set; }
    public IDictionary<string, object> Values { get; set; }

    public AuditData() { }

    private AuditData(EventEntry entry)
    {
        Name = entry.Name;
        Table = entry.Table;
        Schema = entry.Schema;
        if (entry.Action == "Insert") Values = entry.ColumnValues;
        Changes = entry.Changes?.ConvertAll(x => new AuditChange(x)).Where(c => c.New?.ToString() != c.Old?.ToString()).ToList() ?? [];
    }

    public static JsonDocument NewAsJson(EventEntry entry)
    {
        var data = new AuditData(entry);
        return JsonDocument.Parse(data.Serialize());
    }
}
