namespace Exato.Shared.Features.Office.BuscarEmpresas;

public class BuscarEmpresasIn : IApiDto<BuscarEmpresasIn>
{
    public int Page { get; set; }
    public bool? IsActive { get; set; }
    public string? Search { get; set; }
    public MetodoDePagamento? PaymentMethod { get; set; }
    public TipoDeEmpresa? ClientType { get; set; }
    public ExatoWebOnboardStatus? OnboardStatus { get; set; }

    public static IEnumerable<(string, BuscarEmpresasIn)> GetExamples() =>
    [
        ("Exemplo", new BuscarEmpresasIn() { }),
    ];
}
