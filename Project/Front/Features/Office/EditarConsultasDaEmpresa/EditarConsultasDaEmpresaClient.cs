using Exato.Shared.Features.Office.EditarConsultasDaEmpresa;

namespace Exato.Front.Features.Office.EditarConsultasDaEmpresa;

public class EditarConsultasDaEmpresaClient(HttpClient http) : IOfficeClient
{
    public async Task<OneOf<EditarConsultasDaEmpresaOut, ErrorOut>> Editar(
        int id,
        bool highPerformance,
        bool blockSensitiveDataInQueryString,
        DataAccessLevel dataAccessLevel,
        int? transLimitPerWeek,
        bool gerarPdfConsultas,
        bool habilitarConsultasPorEmail,
        bool receitaCpfUseSerproAsMainSource,
        bool receitaCpfShouldReturnMinor18AgeData)
    {
        var body = new EditarConsultasDaEmpresaIn
        {
            HighPerformance = highPerformance,
            BlockSensitiveDataInQueryString = blockSensitiveDataInQueryString,
            DataAccessLevel = dataAccessLevel,
            TransLimitPerWeek = transLimitPerWeek,
            GerarPdfConsultas = gerarPdfConsultas,
            HabilitarConsultasPorEmail = habilitarConsultasPorEmail,
            ReceitaCpfUseSerproAsMainSource = receitaCpfUseSerproAsMainSource,
            ReceitaCpfShouldReturnMinor18AgeData = receitaCpfShouldReturnMinor18AgeData,
        };

        var response = await http.PutAsJsonAsync($"office/empresas/{id}/consultas", body);

        return await response.Resolve<EditarConsultasDaEmpresaOut>();
    }
}
