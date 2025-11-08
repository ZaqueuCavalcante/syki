namespace Exato.Shared.Features.Office.BuscarConsultas;

public class BuscarConsultasIn : IApiDto<BuscarConsultasIn>
{
    public int Page { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public int? TipoId { get; set; }
    public int? ResultadoId { get; set; }
    public int? ClienteId { get; set; }
    public string? Chave { get; set; }
    public string? Document { get; set; }
    public string? Uid { get; set; }

    public static IEnumerable<(string, BuscarConsultasIn)> GetExamples() =>
    [
        ("Exemplo", new BuscarConsultasIn() { }),
    ];
}
