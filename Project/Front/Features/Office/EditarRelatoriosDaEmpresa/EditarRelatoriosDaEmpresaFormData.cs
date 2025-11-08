namespace Exato.Front.Features.Office.EditarRelatoriosDaEmpresa;

public class EditarRelatoriosDaEmpresaFormData
{
    public int Id { get; set; }
    public EditarRelatoriosDaEmpresaItemFormData RelatorioPF { get; set; } = new();
    public EditarRelatoriosDaEmpresaItemFormData RelatorioPFQuod { get; set; } = new();
    public EditarRelatoriosDaEmpresaItemFormData RelatorioPJ { get; set; } = new();
    public EditarRelatoriosDaEmpresaItemFormData RelatorioPJQuod { get; set; } = new();
}

public class EditarRelatoriosDaEmpresaItemFormData
{
    public int Id { get; set; }
    public string Nome { get; set; }
}
