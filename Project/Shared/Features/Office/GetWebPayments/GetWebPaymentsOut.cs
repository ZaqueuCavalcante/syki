namespace Exato.Shared.Features.Office.GetWebPayments;

public class GetWebPaymentsOut : IApiDto<GetWebPaymentsOut>
{
    public int Total { get; set; }
    public List<GetWebPaymentsItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, GetWebPaymentsOut)> GetExamples() =>
    [
        ("Exemplo", new GetWebPaymentsOut() { }),
    ];
}

public class GetWebPaymentsItemOut
{
    public int Id { get; set; }
    public string TransactionCode { get; set; }
    public string User { get; set; }
    public string Company { get; set; }
    public decimal Value { get; set; }
    public decimal Bonus { get; set; }
    public DateTime StartDate { get; set; }
    public WebPaymentStatus Status { get; set; }
    public WebPaymentProvider? PaymentProvider { get; set; }

    public string GetTransactionCode()
    {
        return TransactionCode.HasValue() ? TransactionCode : "-";
    }

    public string GetCompany()
    {
        return Company.HasValue() ? Company : "-";
    }

    public string GetPaymentProvider()
    {
        return PaymentProvider != null ? PaymentProvider.GetDescription() : "-";
    }
}
