using Exato.Front.Auth;
using Microsoft.JSInterop;
using Exato.Shared.Features.Cross.Login;

namespace Exato.Front.Features.Cross.Login;

public class LoginClient(HttpClient http, ILocalStorageService localStorage, ExatoAuthStateProvider authStateProvider) : ICrossClient
{
    public async Task<OneOf<LoginOut, ErrorOut>> Login(string email, string password)
    {
        var body = new LoginIn { Email = email, Password = password };
        var response = await http.PostAsJsonAsync("login", body);

        var result = await response.Resolve<LoginOut>();

        if (result.IsSuccess)
        {
            await localStorage.SetItemAsync("User", result.Success);
            authStateProvider.MarkUserAsAuthenticated();
        }

        return result;
    }
}
