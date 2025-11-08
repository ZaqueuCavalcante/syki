namespace Exato.Shared.Features.Office.GetAuditTrail;

public class GetAuditTrailOut : IApiDto<GetAuditTrailOut>
{
    public string Name { get; set; }
    public string Table { get; set; }
    public string Schema { get; set; }
    public string EntityId { get; set; }

    public string Operation { get; set; }
    public string Action { get; set; }

    public string? User { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<AuditValueOut> Values { get; set; } = [];
    public List<AuditChangeOut> Changes { get; set; } = [];

    public static IEnumerable<(string, GetAuditTrailOut)> GetExamples() =>
    [
        ("Exemplo", new GetAuditTrailOut() { }),
    ];
}

public class AuditValueOut
{
    public string Column { get; set; }
    public object Value { get; set; }
}

public class AuditChangeOut
{
    public string Column { get; set; }
    public object Old { get; set; }
    public object New { get; set; }
}
