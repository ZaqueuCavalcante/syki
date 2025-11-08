namespace Exato.Shared.Features.Office.EditarFaturamentoDaEmpresa;

public class EditarFaturamentoDaEmpresaIn : IApiDto<EditarFaturamentoDaEmpresaIn>
{
    public bool Habilitado { get; set; }
    public MetodoDePagamento MetodoDePagamento { get; set; }

    public static IEnumerable<(string, EditarFaturamentoDaEmpresaIn)> GetExamples() =>
    [
        ("Exemplo", new EditarFaturamentoDaEmpresaIn() { }),
    ];
}
