using Syki.Shared;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Syki.Front.Services;

public class MfaService : IMfaService
{
    private readonly HttpClient _http;
    public MfaService(HttpClient http) => _http = http;

    public async Task<MfaKeyOut> GetMfaKey()
    {
        return (await _http.GetFromJsonAsync<MfaKeyOut>("/users/mfa-key"))!;
    }

    public async Task<MfaSetupOut> EnableUserMfa(string code)
    {
        var data = new MfaSetupIn { Token = code };
        var response = await _http.PostAsJsonAsync("/users/mfa-setup", data);

        var responseAsString = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<MfaSetupOut>(responseAsString)!;
    }
}
