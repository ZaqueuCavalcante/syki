namespace Exato.Shared.Features.Office.BuscarTiposDeResultado;

public class BuscarTiposDeResultadoOut : IApiDto<BuscarTiposDeResultadoOut>
{
    public int Total { get; set; }
    public List<BuscarTiposDeResultadoItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, BuscarTiposDeResultadoOut)> GetExamples() =>
    [
        ("Exemplo", new BuscarTiposDeResultadoOut() { }),
    ];
}

public class BuscarTiposDeResultadoItemOut
{
    public int Id { get; set; }
    public string Nome { get; set; }
}
