namespace Exato.Shared.Features.Office.CriarEmpresa;

public class CriarEmpresaIn : IApiDto<CriarEmpresaIn>
{
    public bool ExatoWeb { get; set; }
    public string Nome { get; set; }
    public string CNPJ { get; set; }
    public string RazaoSocial { get; set; }

    public static IEnumerable<(string, CriarEmpresaIn)> GetExamples() =>
    [
        ("Exemplo", new CriarEmpresaIn() { }),
    ];
}
