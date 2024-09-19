using Microsoft.JSInterop;

namespace Syki.Front.Features.Cross.SkipUserRegister;

public class SkipUserRegisterClient(HttpClient http, ILocalStorageService localStorage, SykiAuthStateProvider authStateProvider) : ICrossClient
{
    public async Task Skip()
    {
        var id = await localStorage.GetItemAsync("UserId");
        _ = Guid.TryParse(id, out Guid userId);

        var response = await http.PostAsJsonAsync("/skip-user-register", new SkipUserRegisterLoginIn { UserId = userId  });
        var result = (await response.Resolve<SkipUserRegisterLoginOut>()).GetSuccess();

        await localStorage.SetItemAsync("AccessToken", result.AccessToken);
        await localStorage.SetItemAsync("UserId", result.UserId.ToString());
        authStateProvider.MarkUserAsAuthenticated();
    }
}
