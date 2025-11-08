using Exato.Shared.Features.Office.BuscarConsultas;

namespace Exato.Front.Features.Office.BuscarConsultas;

public class BuscarConsultasClient(HttpClient http) : IOfficeClient
{
    public async Task<BuscarConsultasOut> Buscar(BuscarConsultasIn query)
    {
        return await http.GetFromJsonAsync<BuscarConsultasOut>("office/consultas".AddQueryString(query)) ?? new();
    }
}
