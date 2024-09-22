namespace Syki.Front.Features.Adm.SetupFeatureFlags;

public class SetupFeatureFlagsClient(HttpClient http) : IAdmClient
{
    public async Task Setup(bool skipUserRegister, bool crossLogin)
    {
        var data = new SetupFeatureFlagsIn { SkipUserRegister = skipUserRegister, CrossLogin = crossLogin };
        await http.PutAsJsonAsync("/adm/feature-flags", data);
    }
}
