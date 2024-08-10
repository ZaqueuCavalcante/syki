namespace Syki.Front.Features.Cross.CreatePendingUserRegister;

public class CreatePendingUserRegisterClient(HttpClient http) : ICrossClient
{
    public async Task<OneOf<SuccessOut, ErrorOut>> Create(string email)
    {
        var data = new CreatePendingUserRegisterIn(email);

        var response = await http.PostAsJsonAsync("/users", data);

        return await response.Resolve<SuccessOut>();
    }
}
