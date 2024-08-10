namespace Syki.Front.Features.Adm.GetUsers;

public class GetUsersClient(HttpClient http) : IAdmClient
{
    public async Task<List<UserOut>> Get()
    {
        return await http.GetFromJsonAsync<List<UserOut>>("/adm/users") ?? [];
    }
}
