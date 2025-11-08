namespace Exato.Shared.Features.Office.BuscarEventosDoUsuario;

public class BuscarEventosDoUsuarioOut : IApiDto<BuscarEventosDoUsuarioOut>
{
    public int Total { get; set; }
    public List<BuscarEventosDoUsuarioItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, BuscarEventosDoUsuarioOut)> GetExamples() =>
    [
        ("Exemplo", new BuscarEventosDoUsuarioOut() { }),
    ];
}

public class BuscarEventosDoUsuarioItemOut
{
    public DateTime EventDate { get; set; }
    public string Description { get; set; }
}
