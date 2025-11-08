namespace Exato.Shared.Features.Office.BuscarPotenciaisMatrizesDaEmpresa;

public class BuscarPotenciaisMatrizesDaEmpresaIn : IApiDto<BuscarPotenciaisMatrizesDaEmpresaIn>
{
    public string? CnpjOuNome { get; set; }

    public static IEnumerable<(string, BuscarPotenciaisMatrizesDaEmpresaIn)> GetExamples() =>
    [
        ("Exemplo", new BuscarPotenciaisMatrizesDaEmpresaIn() { }),
    ];
}
