using Exato.Shared.Features.Office.BuscarVendedores;

namespace Exato.Front.Features.Office.BuscarVendedores;

public class BuscarVendedoresClient(HttpClient http) : IOfficeClient
{
    public async Task<BuscarVendedoresOut> Get()
    {
        return await http.GetFromJsonAsync<BuscarVendedoresOut>("office/vendedores") ?? new();
    }
}
