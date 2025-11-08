namespace Exato.Shared.Features.Office.BuscarPotenciaisEmpresasDoUsuario;

public class BuscarPotenciaisEmpresasDoUsuarioIn : IApiDto<BuscarPotenciaisEmpresasDoUsuarioIn>
{
    public string? CnpjOuNome { get; set; }

    public static IEnumerable<(string, BuscarPotenciaisEmpresasDoUsuarioIn)> GetExamples() =>
    [
        ("Exemplo", new BuscarPotenciaisEmpresasDoUsuarioIn() { }),
    ];
}
