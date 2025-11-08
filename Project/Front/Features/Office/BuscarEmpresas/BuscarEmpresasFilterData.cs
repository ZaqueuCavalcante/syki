namespace Exato.Front.Features.Office.BuscarEmpresas;

public class BuscarEmpresasFilterData
{
    public bool? IsActive { get; set; }
    public string? Search { get; set; }
    public MetodoDePagamento? PaymentMethod { get; set; }
    public TipoDeEmpresa? ClientType { get; set; }
    public ExatoWebOnboardStatus? OnboardStatus { get; set; }
}
