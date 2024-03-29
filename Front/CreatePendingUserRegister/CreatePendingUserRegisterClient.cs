namespace Syki.Front.CreatePendingUserRegister;

public class CreatePendingUserRegisterClient(HttpClient http)
{
    public async Task<HttpResponseMessage> Create(string email)
    {
        var data = new CreatePendingUserRegisterIn { Email = email };
        return await http.PostAsJsonAsync("/users", data);
    }
}
