namespace Exato.Shared.Features.Office.EditarRelatoriosDaEmpresa;

public class EditarRelatoriosDaEmpresaOut : IApiDto<EditarRelatoriosDaEmpresaOut>
{
    public int Id { get; set; }

    public static IEnumerable<(string, EditarRelatoriosDaEmpresaOut)> GetExamples() =>
    [
        ("Exemplo", new EditarRelatoriosDaEmpresaOut() { }),
    ];
}
