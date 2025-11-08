namespace Exato.Shared.Features.Office.BuscarPotenciaisMatrizesDaEmpresa;

public class BuscarPotenciaisMatrizesDaEmpresaOut : IApiDto<BuscarPotenciaisMatrizesDaEmpresaOut>
{
    public List<BuscarPotenciaisMatrizesDaEmpresaItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, BuscarPotenciaisMatrizesDaEmpresaOut)> GetExamples() =>
    [
        ("Exemplo", new BuscarPotenciaisMatrizesDaEmpresaOut() { }),
    ];
}

public class BuscarPotenciaisMatrizesDaEmpresaItemOut
{
    public int Id { get; set; }
    public bool Ativa { get; set; }
    public string CNPJ { get; set; }
    public string Nome { get; set; }
}
