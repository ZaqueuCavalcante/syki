using Exato.Shared.Features.Office.BuscarTokensDeAcesso;

namespace Exato.Front.Features.Office.BuscarTokensDeAcesso;

public class BuscarTokensDeAcessoClient(HttpClient http) : IOfficeClient
{
    public async Task<BuscarTokensDeAcessoOut> Get(BuscarTokensDeAcessoIn query)
    {
        return await http.GetFromJsonAsync<BuscarTokensDeAcessoOut>("office/tokens".AddQueryString(query), HttpConfigs.JsonOptions) ?? new();
    }
}
