using Audit.EntityFramework;

namespace Syki.Back.Audit;

public class AuditChange
{
    public string Column { get; set; }
    public object Old { get; set; }
    public object New { get; set; }

    public AuditChange(EventEntryChange change)
    {
        Column = change.ColumnName;
        Old = change.OriginalValue;
        New = change.NewValue;
    }
}
