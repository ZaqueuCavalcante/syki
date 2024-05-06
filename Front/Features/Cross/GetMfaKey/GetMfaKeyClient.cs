namespace Syki.Front.Features.Cross.GetMfaKey;

public class GetMfaKeyClient(HttpClient http)
{
    public async Task<GetMfaKeyOut> Get()
    {
        return await http.GetFromJsonAsync<GetMfaKeyOut>("/mfa/key") ?? new();
    }
}
