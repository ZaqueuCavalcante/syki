namespace Exato.Shared.Features.Office.EditarCadastroDaEmpresa;

public class EditarCadastroDaEmpresaIn : IApiDto<EditarCadastroDaEmpresaIn>
{
    public bool Ativa { get; set; }
    public string Nome { get; set; }
    public string CNPJ { get; set; }
    public string RazaoSocial { get; set; }
    public int? MatrizId { get; set; }
    public string? NomeFantasia { get; set; }
    public string? Slug { get; set; }
    public string? SalesContact { get; set; }

    public static IEnumerable<(string, EditarCadastroDaEmpresaIn)> GetExamples() =>
    [
        ("Exemplo", new EditarCadastroDaEmpresaIn() { }),
    ];
}
