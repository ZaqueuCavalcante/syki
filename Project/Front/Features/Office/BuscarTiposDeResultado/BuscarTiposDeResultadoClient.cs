using Exato.Shared.Features.Office.BuscarTiposDeResultado;

namespace Exato.Front.Features.Office.BuscarTiposDeResultado;

public class BuscarTiposDeResultadoClient(HttpClient http) : IOfficeClient
{
    public async Task<BuscarTiposDeResultadoOut> Get(BuscarTiposDeResultadoIn data)
    {
        return await http.GetFromJsonAsync<BuscarTiposDeResultadoOut>("office/tipos-de-resultado".AddQueryString(data), HttpConfigs.JsonOptions) ?? new();
    }
}
