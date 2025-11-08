namespace Exato.Shared.Features.Office.BuscarConsultas;

public class BuscarConsultasOut : IApiDto<BuscarConsultasOut>
{
    public int Total { get; set; }
    public List<BuscarConsultasItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, BuscarConsultasOut)> GetExamples() =>
    [
        ("Exemplo", new BuscarConsultasOut() { }),
    ];
}

public class BuscarConsultasItemOut
{
    public string UId { get; set; }
    public int TipoId { get; set; }
    public string Tipo { get; set; }
    public string Cliente { get; set; }
    public DateTime Inicio { get; set; }
    public DateTime? Fim { get; set; }
    public string Resultado { get; set; }
    public bool HasPdf { get; set; }
    public string PdfPassword { get; set; }

    public string GetTipo()
    {
        return $"{Tipo} - [{TipoId}]";
    }
}
