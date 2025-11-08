namespace Exato.Shared.Features.Office.BuscarTiposDeConsulta;

public class BuscarTiposDeConsultaIn : IApiDto<BuscarTiposDeConsultaIn>
{
    public string? Nome { get; set; }

    public static IEnumerable<(string, BuscarTiposDeConsultaIn)> GetExamples() =>
    [
        ("Exemplo", new BuscarTiposDeConsultaIn() { }),
    ];
}
