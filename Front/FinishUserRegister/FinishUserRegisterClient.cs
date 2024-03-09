namespace Syki.Front.FinishUserRegister;

public class FinishUserRegisterClient(HttpClient http)
{
    public async Task<HttpResponseMessage> Finish(string? token, string password)
    {
        var data = new FinishUserRegisterIn { Token = token, Password = password };
        return await http.PutAsJsonAsync("/user-register", data);
    }
}
