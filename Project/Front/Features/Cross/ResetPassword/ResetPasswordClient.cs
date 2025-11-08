using Exato.Shared.Features.Cross.ResetPassword;

namespace Exato.Front.Features.Cross.ResetPassword;

public class ResetPasswordClient(HttpClient http) : ICrossClient
{
    public async Task<HttpResponseMessage> Reset(string? token, string password)
    {
        var data = new ResetPasswordIn { Token = token, Password = password };
        return await http.PostAsJsonAsync("reset-password", data);
    }
}
