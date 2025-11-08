using Exato.Front.Components.Consultas;
using Exato.Front.Components.Empresas;

namespace Exato.Front.Features.Office.BuscarConsultas;

public class BuscarConsultasFilterData
{
    public DateTime? StartDate { get; set; } = DateTime.Now.Date;
    public TimeSpan? StartTime { get; set; } = new TimeSpan(00, 00, 00);
    public DateTime? EndDate { get; set; } = DateTime.Now.Date;
    public TimeSpan? EndTime { get; set; } = new TimeSpan(23, 59, 59);

    public int? ClienteId { get; set; }
    public string? ClienteName { get; set; }
    public EmpresaSelectData Cliente { get; set; } = new();

    public int? TipoId { get; set; }
    public string? TipoName { get; set; }
    public TipoDeConsultaSelectData Tipo { get; set; } = new();

    public int? ResultadoId { get; set; }
    public string? ResultadoName { get; set; }
    public TipoDeResultadoSelectData Resultado { get; set; } = new();

    public string? Uid { get; set; }
    public string? Chave { get; set; }
    public string? Document { get; set; }
}
