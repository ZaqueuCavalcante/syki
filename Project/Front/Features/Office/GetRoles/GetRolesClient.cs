using Exato.Shared.Features.Office.GetRoles;

namespace Exato.Front.Features.Office.GetRoles;

public class GetRolesClient(HttpClient http) : IOfficeClient
{
    public async Task<GetRolesOut> Get(GetRolesIn query)
    {
        return await http.GetFromJsonAsync<GetRolesOut>($"office/roles".AddQueryString(query), HttpConfigs.JsonOptions) ?? new();
    }
}
