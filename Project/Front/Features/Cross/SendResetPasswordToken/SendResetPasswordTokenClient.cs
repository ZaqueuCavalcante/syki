using Exato.Shared.Features.Cross.SendResetPasswordToken;

namespace Exato.Front.Features.Cross.SendResetPasswordToken;

public class SendResetPasswordTokenClient(HttpClient http) : ICrossClient
{
    public async Task<HttpResponseMessage> Send(string email)
    {
        var data = new SendResetPasswordTokenIn { Email = email };
        return await http.PostAsJsonAsync("reset-password-token", data);
    }
}
