namespace Exato.Shared.Features.Office.EditarFaturamentoDaEmpresa;

public class EditarFaturamentoDaEmpresaOut : IApiDto<EditarFaturamentoDaEmpresaOut>
{
    public int Id { get; set; }

    public static IEnumerable<(string, EditarFaturamentoDaEmpresaOut)> GetExamples() =>
    [
        ("Exemplo", new EditarFaturamentoDaEmpresaOut() { }),
    ];
}
