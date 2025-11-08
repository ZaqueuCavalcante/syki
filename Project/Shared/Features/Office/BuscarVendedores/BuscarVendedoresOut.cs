namespace Exato.Shared.Features.Office.BuscarVendedores;

public class BuscarVendedoresOut : IApiDto<BuscarVendedoresOut>
{
    public List<string> Items { get; set; } = [];

    public static IEnumerable<(string, BuscarVendedoresOut)> GetExamples() =>
    [
        ("Exemplo", new BuscarVendedoresOut() { }),
    ];
}
