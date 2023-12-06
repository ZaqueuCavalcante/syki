using Newtonsoft.Json;
using System.Text.Json;
using Audit.EntityFramework;

namespace Syki.Back.Audit;

public class AuditData
{
    public string Name { get; set; }
    public string Table { get; set; }
    public string Action { get; set; }
    public string Schema { get; set; }
    public List<AuditChange> Changes { get; set; }
    public IDictionary<string, object> Values { get; set; }

    public AuditData() { }

    private AuditData(EventEntry entry)
    {
        Name = entry.Name;
        Table = entry.Table;
        Action = entry.Action;
        Schema = entry.Schema;
        Changes = entry.Changes?.ConvertAll(x => new AuditChange(x)) ?? [];
        Values = entry.ColumnValues;
    }

    public static JsonDocument NewAsJson(EventEntry entry)
    {
        var data = new AuditData(entry);
        var serialized = JsonConvert.SerializeObject(data);
        return JsonDocument.Parse(serialized);
    }
}
