using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;

namespace Syki.Front.Features.Academic.CrossLogin;

public class CrossLoginClient(HttpClient http, ILocalStorageService localStorage, SykiAuthStateProvider authStateProvider) : ICrossClient
{
    public async Task<CrossLoginOut> Login(Guid targetUserId)
    {
        var body = new CrossLoginIn { TargetUserId = targetUserId };
        var response = await http.PostAsJsonAsync("/academic/cross-login", body);

        var result = await response.Resolve<CrossLoginOut>();
        if (result.IsSuccess())
        {
            await localStorage.RemoveItemAsync("AccessToken");
            await localStorage.SetItemAsync("AccessToken", result.GetSuccess().AccessToken);
            authStateProvider.MarkUserAsAuthenticated();
        }

        return result.GetSuccess();
    }
}
