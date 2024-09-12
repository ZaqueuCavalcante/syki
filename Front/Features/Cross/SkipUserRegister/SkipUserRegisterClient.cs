using Microsoft.JSInterop;

namespace Syki.Front.Features.Cross.SkipUserRegister;

public class SkipUserRegisterClient(HttpClient http, ILocalStorageService localStorage, SykiAuthStateProvider authStateProvider) : ICrossClient
{
    public async Task Skip()
    {
        var id = await localStorage.GetItemAsync("InstitutionId");
        _ = Guid.TryParse(id, out Guid institutionId);

        var response = await http.PostAsJsonAsync("/skip-user-register", new SkipRegisterLoginIn { InstitutionId = institutionId  });
        var result = (await response.Resolve<SkipRegisterLoginOut>()).GetSuccess();

        await localStorage.SetItemAsync("AccessToken", result.AccessToken);
        await localStorage.SetItemAsync("InstitutionId", result.InstitutionId.ToString());
        authStateProvider.MarkUserAsAuthenticated();
    }
}
