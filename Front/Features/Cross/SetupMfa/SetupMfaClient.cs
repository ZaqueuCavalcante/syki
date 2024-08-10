namespace Syki.Front.Features.Cross.SetupMfa;

public class SetupMfaClient(HttpClient http) : ICrossClient
{
    public async Task<bool> Setup(string token)
    {
        var data = new SetupMfaIn { Token = token };
        var response = await http.PostAsJsonAsync("/mfa/setup", data);
        return response.IsSuccessStatusCode;
    }
}
