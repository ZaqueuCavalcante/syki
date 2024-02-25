using System.Net.Http.Json;
using Syki.Shared.SetupMfa;
using Syki.Shared.GetMfaKey;

namespace Syki.Front.Services;

public class MfaService(HttpClient http) : IMfaService
{
    public async Task<GetMfaKeyOut> GetMfaKey()
    {
        return (await http.GetFromJsonAsync<GetMfaKeyOut>("/mfa/key"))!;
    }

    public async Task<bool> EnableUserMfa(string code)
    {
        var data = new SetupMfaIn { Token = code };
        var response = await http.PostAsJsonAsync("/mfa/setup", data);

        return response.IsSuccessStatusCode;
    }
}
