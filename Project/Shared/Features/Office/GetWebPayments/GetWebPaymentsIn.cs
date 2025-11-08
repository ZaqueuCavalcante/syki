namespace Exato.Shared.Features.Office.GetWebPayments;

public class GetWebPaymentsIn : IApiDto<GetWebPaymentsIn>
{
    public int Page { get; set; }

    public static IEnumerable<(string, GetWebPaymentsIn)> GetExamples() =>
    [
        ("Exemplo", new GetWebPaymentsIn() { }),
    ];
}
