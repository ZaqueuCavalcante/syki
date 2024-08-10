namespace Syki.Front.Features.Cross.FinishUserRegister;

public class FinishUserRegisterClient(HttpClient http) : ICrossClient
{
    public async Task<HttpResponseMessage> Finish(string? token, string password)
    {
        var data = new FinishUserRegisterIn(token, password);
        return await http.PutAsJsonAsync("/users", data);
    }
}
