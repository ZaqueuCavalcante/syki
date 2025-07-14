namespace Syki.Front.Features.Cross.Logout;

public class LogoutClient(HttpClient http) : ICrossClient
{
    public async Task<HttpResponseMessage> Logout()
    {
        return await http.PostAsJsonAsync("/logout", new {});
    }
}
