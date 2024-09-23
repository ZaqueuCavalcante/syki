using Microsoft.JSInterop;

namespace Syki.Front.Features.Academic.CrossLogin;

public class CrossLoginClient(HttpClient http, ILocalStorageService localStorage, SykiAuthStateProvider authStateProvider) : ICrossClient
{
    public async Task<OneOf<CrossLoginOut, ErrorOut>> Login(Guid targetUserId)
    {
        var body = new CrossLoginIn { TargetUserId = targetUserId };
        var response = await http.PostAsJsonAsync("/academic/cross-login", body);

        var result = await response.Resolve<CrossLoginOut>();

        if (result.IsError()) return result.GetError();

        await localStorage.SetItemAsync("AccessToken", result.GetSuccess().AccessToken);
        authStateProvider.MarkUserAsAuthenticated();

        return result;
    }
}
