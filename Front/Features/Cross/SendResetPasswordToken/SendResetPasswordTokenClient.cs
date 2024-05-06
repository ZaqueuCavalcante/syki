namespace Syki.Front.Features.Cross.SendResetPasswordToken;

public class SendResetPasswordTokenClient(HttpClient http)
{
    public async Task<HttpResponseMessage> Send(string email)
    {
        var data = new SendResetPasswordTokenIn { Email = email };
        return await http.PostAsJsonAsync("/reset-password-token", data);
    }
}
