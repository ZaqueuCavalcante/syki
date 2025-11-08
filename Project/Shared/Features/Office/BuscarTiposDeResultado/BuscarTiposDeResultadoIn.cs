namespace Exato.Shared.Features.Office.BuscarTiposDeResultado;

public class BuscarTiposDeResultadoIn : IApiDto<BuscarTiposDeResultadoIn>
{
    public int Page { get; set; }
    public string? Nome { get; set; }

    public static IEnumerable<(string, BuscarTiposDeResultadoIn)> GetExamples() =>
    [
        ("Exemplo", new BuscarTiposDeResultadoIn() { }),
    ];
}
