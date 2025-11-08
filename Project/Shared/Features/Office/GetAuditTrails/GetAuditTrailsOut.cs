namespace Exato.Shared.Features.Office.GetAuditTrails;

public class GetAuditTrailsOut : IApiDto<GetAuditTrailsOut>
{
    public int Total { get; set; }
    public List<GetAuditTrailsItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetAuditTrailsOut)> GetExamples() =>
    [
        ("Exemplo", new GetAuditTrailsOut() { }),
    ];
}

public class GetAuditTrailsItemOut
{
    public Guid Id { get; set; }

    public string ActivityId { get; set; }
    public string Operation { get; set; }

    public string EntityType { get; set; }

    public Guid? UserId { get; set; }
    public string? User { get; set; }

    public string Action { get; set; }
    public DateTime CreatedAt { get; set; }
}
