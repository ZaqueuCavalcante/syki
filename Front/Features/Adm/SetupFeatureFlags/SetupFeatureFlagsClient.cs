namespace Syki.Front.Features.Adm.SetupFeatureFlags;

public class SetupFeatureFlagsClient(HttpClient http) : IAdmClient
{
    public async Task Setup(bool crossLogin)
    {
        var data = new SetupFeatureFlagsIn { CrossLogin = crossLogin };
        await http.PutAsJsonAsync("/adm/feature-flags", data);
    }
}
