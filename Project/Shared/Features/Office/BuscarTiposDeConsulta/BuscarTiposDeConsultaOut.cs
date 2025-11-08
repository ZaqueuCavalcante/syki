namespace Exato.Shared.Features.Office.BuscarTiposDeConsulta;

public class BuscarTiposDeConsultaOut : IApiDto<BuscarTiposDeConsultaOut>
{
    public int Total { get; set; }
    public List<BuscarTiposDeConsultaItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, BuscarTiposDeConsultaOut)> GetExamples() =>
    [
        ("Exemplo", new BuscarTiposDeConsultaOut() { }),
    ];
}

public class BuscarTiposDeConsultaItemOut
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public bool Visivel { get; set; }
    public bool Disponivel { get; set; }
}
