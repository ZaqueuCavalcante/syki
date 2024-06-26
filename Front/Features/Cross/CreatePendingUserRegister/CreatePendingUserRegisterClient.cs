namespace Syki.Front.Features.Cross.CreatePendingUserRegister;

public class CreatePendingUserRegisterClient(HttpClient http)
{
    public async Task<HttpResponseMessage> Create(string email)
    {
        var data = new CreatePendingUserRegisterIn(email);
        return await http.PostAsJsonAsync("/users", data);
    }
}
