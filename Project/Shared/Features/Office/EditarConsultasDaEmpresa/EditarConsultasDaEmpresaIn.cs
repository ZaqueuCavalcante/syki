namespace Exato.Shared.Features.Office.EditarConsultasDaEmpresa;

public class EditarConsultasDaEmpresaIn : IApiDto<EditarConsultasDaEmpresaIn>
{
    public bool HighPerformance { get; set; }
    public bool BlockSensitiveDataInQueryString { get; set; }
    public DataAccessLevel DataAccessLevel { get; set; }
    public int? TransLimitPerWeek { get; set; }

    public bool GerarPdfConsultas { get; set; }
    public bool HabilitarConsultasPorEmail { get; set; }

    public bool ReceitaCpfUseSerproAsMainSource { get; set; }
    public bool ReceitaCpfShouldReturnMinor18AgeData { get; set; }

    public static IEnumerable<(string, EditarConsultasDaEmpresaIn)> GetExamples() =>
    [
        ("Exemplo", new EditarConsultasDaEmpresaIn() { }),
    ];
}
