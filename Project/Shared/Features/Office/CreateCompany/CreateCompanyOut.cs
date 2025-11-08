namespace Exato.Shared.Features.Office.CreateCompany;

public class CreateCompanyOut : IApiDto<CreateCompanyOut>
{
    public int Id { get; set; }

    public static IEnumerable<(string, CreateCompanyOut)> GetExamples() =>
    [
        ("Exemplo", new CreateCompanyOut() { }),
    ];
}
