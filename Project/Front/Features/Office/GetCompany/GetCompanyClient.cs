using Exato.Shared.Features.Office.GetCompany;

namespace Exato.Front.Features.Office.GetCompany;

public class GetCompanyClient(HttpClient http) : IOfficeClient
{
    public async Task<GetCompanyOut> Get(Guid id)
    {
        return await http.GetFromJsonAsync<GetCompanyOut>($"office/companies/{id}", HttpConfigs.JsonOptions) ?? new();
    }
}
