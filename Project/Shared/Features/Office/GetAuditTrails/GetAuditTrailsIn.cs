namespace Exato.Shared.Features.Office.GetAuditTrails;

public class GetAuditTrailsIn : IApiDto<GetAuditTrailsIn>
{
    public int Page { get; set; }
    public string? Operation { get; set; }
    public string? Action { get; set; }
    public Guid? UserId { get; set; }

    public static IEnumerable<(string, GetAuditTrailsIn)> GetExamples() =>
    [
        ("Exemplo", new GetAuditTrailsIn() { }),
    ];
}
