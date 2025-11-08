namespace Exato.Shared.Features.Office.GetCompany;

public class GetCompanyOut : IApiDto<GetCompanyOut>
{
    public int Id { get; set; }
    public Guid ExternalId { get; set; }
    public string Name { get; set; }
    public bool Active { get; set; }
    public string Cnpj { get; set; }
    public CompanyPaymentMode PaymentMode { get; set; }
    public ExatoWebOnboardStatus OnboardStatus { get; set; }
    public DateTime OnboardDate { get; set; }

    public static IEnumerable<(string, GetCompanyOut)> GetExamples() =>
    [
        ("Exemplo", new GetCompanyOut() { }),
    ];
}
