using Exato.Shared.Features.Office.GetOrganizationRoles;

namespace Exato.Front.Features.Office.GetOrganizationRoles;

public class GetOrganizationRolesClient(HttpClient http) : IOfficeClient
{
    public async Task<GetOrganizationRolesOut> Get(int id, GetOrganizationRolesIn query)
    {
        return await http.GetFromJsonAsync<GetOrganizationRolesOut>($"office/empresas/{id}/roles".AddQueryString(query), HttpConfigs.JsonOptions) ?? new();
    }
}
