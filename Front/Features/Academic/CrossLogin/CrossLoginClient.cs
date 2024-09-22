using Microsoft.JSInterop;

namespace Syki.Front.Features.Academic.CrossLogin;

public class CrossLoginClient(HttpClient http, ILocalStorageService localStorage, SykiAuthStateProvider authStateProvider) : ICrossClient
{
    public async Task<OneOf<CrossLoginOut, ErrorOut>> Login(Guid targetUserId)
    {
        var body = new CrossLoginIn { TargetUserId = targetUserId };
        var response = await http.PostAsJsonAsync("/academic/cross-login", body);

        if (!response.IsSuccessStatusCode) return new ErrorOut();

        var result = (await response.Resolve<CrossLoginOut>()).GetSuccess();
        await localStorage.SetItemAsync("AccessToken", result.AccessToken);
        authStateProvider.MarkUserAsAuthenticated();

        return result;
    }
}
