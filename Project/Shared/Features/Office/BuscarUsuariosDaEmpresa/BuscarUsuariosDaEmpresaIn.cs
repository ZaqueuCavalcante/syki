namespace Exato.Shared.Features.Office.BuscarUsuariosDaEmpresa;

public class BuscarUsuariosDaEmpresaIn : IApiDto<BuscarUsuariosDaEmpresaIn>
{
    public int Page { get; set; }
    public bool? IsActive { get; set; }
    public string? SearchTerm { get; set; }

    public static IEnumerable<(string, BuscarUsuariosDaEmpresaIn)> GetExamples() =>
    [
        ("Exemplo", new BuscarUsuariosDaEmpresaIn() { }),
    ];
}
