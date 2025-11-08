namespace Exato.Shared.Features.Office.EditarRelatoriosDaEmpresa;

public class EditarRelatoriosDaEmpresaIn : IApiDto<EditarRelatoriosDaEmpresaIn>
{
    public int Pf { get; set; }
    public int Pj { get; set; }
    public int PfQuod { get; set; }
    public int PjQuod { get; set; }

    public static IEnumerable<(string, EditarRelatoriosDaEmpresaIn)> GetExamples() =>
    [
        ("Exemplo", new EditarRelatoriosDaEmpresaIn() { }),
    ];
}
