namespace Exato.Shared.Features.Office.BuscarTiposDeRelatorios;

public class BuscarTiposDeRelatoriosOut : IApiDto<BuscarTiposDeRelatoriosOut>
{
    public List<BuscarTiposDeRelatoriosItemOut> Pf { get; set; } = [];
    public List<BuscarTiposDeRelatoriosItemOut> Pj { get; set; } = [];
    public List<BuscarTiposDeRelatoriosItemOut> PfQuod { get; set; } = [];
    public List<BuscarTiposDeRelatoriosItemOut> PjQuod { get; set; } = [];

    public static IEnumerable<(string, BuscarTiposDeRelatoriosOut)> GetExamples() =>
    [
        ("Exemplo", new BuscarTiposDeRelatoriosOut() { }),
    ];
}

public class BuscarTiposDeRelatoriosItemOut
{
    public int Id { get; set; }
    public string Nome { get; set; }
}
