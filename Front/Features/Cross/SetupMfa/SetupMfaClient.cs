namespace Syki.Front.SetupMfa;

public class SetupMfaClient(HttpClient http)
{
    public async Task<bool> Setup(string code)
    {
        var data = new SetupMfaIn { Token = code };
        var response = await http.PostAsJsonAsync("/mfa/setup", data);
        return response.IsSuccessStatusCode;
    }
}
