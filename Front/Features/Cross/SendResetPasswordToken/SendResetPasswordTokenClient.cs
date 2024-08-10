namespace Syki.Front.Features.Cross.SendResetPasswordToken;

public class SendResetPasswordTokenClient(HttpClient http) : ICrossClient
{
    public async Task<HttpResponseMessage> Send(string email)
    {
        var data = new SendResetPasswordTokenIn(email);
        return await http.PostAsJsonAsync("/reset-password-token", data);
    }
}
