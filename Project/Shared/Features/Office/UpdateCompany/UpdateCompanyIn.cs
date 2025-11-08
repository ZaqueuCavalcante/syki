namespace Exato.Shared.Features.Office.UpdateCompany;

public class UpdateCompanyIn : IApiDto<UpdateCompanyIn>
{
    public ExatoWebOnboardStatus OnboardStatus { get; set; }

    public static IEnumerable<(string, UpdateCompanyIn)> GetExamples() =>
    [
        ("Exemplo", new UpdateCompanyIn() { }),
    ];
}
