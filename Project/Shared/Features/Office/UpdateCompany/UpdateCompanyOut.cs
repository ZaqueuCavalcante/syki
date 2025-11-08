namespace Exato.Shared.Features.Office.UpdateCompany;

public class UpdateCompanyOut : IApiDto<UpdateCompanyOut>
{
    public static IEnumerable<(string, UpdateCompanyOut)> GetExamples() =>
    [
        ("Exemplo", new UpdateCompanyOut() { }),
    ];
}
