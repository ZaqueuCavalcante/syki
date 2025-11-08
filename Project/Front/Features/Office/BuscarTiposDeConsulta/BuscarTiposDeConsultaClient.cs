using Exato.Shared.Features.Office.BuscarTiposDeConsulta;

namespace Exato.Front.Features.Office.BuscarTiposDeConsulta;

public class BuscarTiposDeConsultaClient(HttpClient http) : IOfficeClient
{
    public async Task<BuscarTiposDeConsultaOut> Get(BuscarTiposDeConsultaIn data)
    {
        return await http.GetFromJsonAsync<BuscarTiposDeConsultaOut>("office/tipos-de-consulta".AddQueryString(data), HttpConfigs.JsonOptions) ?? new();
    }
}
