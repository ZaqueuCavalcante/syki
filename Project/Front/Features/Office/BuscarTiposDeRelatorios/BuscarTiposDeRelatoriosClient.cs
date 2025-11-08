using Exato.Shared.Features.Office.BuscarTiposDeRelatorios;

namespace Exato.Front.Features.Office.BuscarTiposDeRelatorios;

public class BuscarTiposDeRelatoriosClient(HttpClient http) : IOfficeClient
{
    public async Task<BuscarTiposDeRelatoriosOut> Get()
    {
        return await http.GetFromJsonAsync<BuscarTiposDeRelatoriosOut>("office/tipos-de-relatorios") ?? new();
    }
}
