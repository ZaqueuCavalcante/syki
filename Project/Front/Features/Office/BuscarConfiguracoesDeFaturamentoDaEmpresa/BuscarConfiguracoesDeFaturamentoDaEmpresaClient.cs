using Exato.Shared.Features.Office.BuscarConfiguracoesDeFaturamentoDaEmpresa;

namespace Exato.Front.Features.Office.BuscarConfiguracoesDeFaturamentoDaEmpresa;

public class BuscarConfiguracoesDeFaturamentoDaEmpresaClient(HttpClient http) : IOfficeClient
{
    public async Task<BuscarConfiguracoesDeFaturamentoDaEmpresaOut> Get(int id)
    {
        return await http.GetFromJsonAsync<BuscarConfiguracoesDeFaturamentoDaEmpresaOut>($"office/empresas/{id}/faturamento/configs", HttpConfigs.JsonOptions) ?? new();
    }
}
