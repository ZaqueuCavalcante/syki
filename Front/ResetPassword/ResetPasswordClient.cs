namespace Syki.Front.ResetPassword;

public class ResetPasswordClient(HttpClient http)
{
    public async Task<HttpResponseMessage> Reset(string? token, string password)
    {
        var data = new ResetPasswordIn { Token = token, Password = password };
        return await http.PostAsJsonAsync("/reset-password", data);
    }
}
