namespace Exato.Shared.Features.Office.CreateCompany;

public class CreateCompanyIn : IApiDto<CreateCompanyIn>
{
    public Guid ExternalId { get; set; }

    public static IEnumerable<(string, CreateCompanyIn)> GetExamples() =>
    [
        ("Exemplo", new CreateCompanyIn() { }),
    ];
}
