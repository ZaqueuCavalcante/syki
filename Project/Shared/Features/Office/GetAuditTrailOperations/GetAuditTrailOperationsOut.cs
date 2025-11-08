namespace Exato.Shared.Features.Office.GetAuditTrailOperations;

public class GetAuditTrailOperationsOut : IApiDto<GetAuditTrailOperationsOut>
{
    public List<string> Items { get; set; } = [];

    public static IEnumerable<(string, GetAuditTrailOperationsOut)> GetExamples() =>
    [
        ("Exemplo", new GetAuditTrailOperationsOut() { }),
    ];
}
