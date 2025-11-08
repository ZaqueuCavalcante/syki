namespace Exato.Shared.Features.Office.CriarEmpresa;

public class CriarEmpresaOut : IApiDto<CriarEmpresaOut>
{
    public int Id { get; set; }
    public Guid ExternalId { get; set; }

    public static IEnumerable<(string, CriarEmpresaOut)> GetExamples() =>
    [
        ("Exemplo", new CriarEmpresaOut() { }),
    ];
}
