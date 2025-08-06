using Microsoft.JSInterop;

namespace Syki.Front.Features.Cross.Login;

public class LoginClient(HttpClient http, ILocalStorageService localStorage, SykiAuthStateProvider authStateProvider) : ICrossClient
{
    public async Task<OneOf<LoginOut, ErrorOut>> Login(string email, string password)
    {
        var body = new LoginIn(email, password);
        var response = await http.PostAsJsonAsync("/login", body);

        var result = await response.Resolve<LoginOut>();

        if (result.IsSuccess)
        {
            await localStorage.SetItemAsync("User", result.Success);
            authStateProvider.MarkUserAsAuthenticated();
        }

        return result;
    }
}
