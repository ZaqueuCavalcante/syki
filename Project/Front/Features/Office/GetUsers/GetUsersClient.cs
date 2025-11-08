using Exato.Shared.Features.Office.GetUsers;

namespace Exato.Front.Features.Office.GetUsers;

public class GetUsersClient(HttpClient http) : IOfficeClient
{
    public async Task<GetUsersOut> Get(GetUsersIn query)
    {
        return await http.GetFromJsonAsync<GetUsersOut>("office/users".AddQueryString(query), HttpConfigs.JsonOptions) ?? new();
    }
}
