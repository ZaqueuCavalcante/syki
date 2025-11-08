namespace Exato.Shared.Features.Office.BuscarConsulta;

public class BuscarConsultaOut : IApiDto<BuscarConsultaOut>
{
    public string UId { get; set; }
    public string MasterUId { get; set; }
    public int TipoId { get; set; }
    public string Tipo { get; set; }
    public string Cliente { get; set; }
    public DateTime Inicio { get; set; }
    public DateTime? Fim { get; set; }
    public string Resultado { get; set; }
    public bool HasPdf { get; set; }
    public string PdfPassword { get; set; }

    public List<BuscarConsultaOut> Subconsultas { get; set; } = [];

    public string GetTipo()
    {
        return $"{Tipo} - [{TipoId}]";
    }

    public static IEnumerable<(string, BuscarConsultaOut)> GetExamples() =>
    [
        ("Exemplo", new BuscarConsultaOut() { }),
    ];
}
