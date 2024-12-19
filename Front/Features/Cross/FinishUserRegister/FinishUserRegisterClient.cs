namespace Syki.Front.Features.Cross.FinishUserRegister;

public class FinishUserRegisterClient(HttpClient http) : ICrossClient
{
    public async Task<OneOf<UserOut, ErrorOut>> Finish(string? token, string password)
    {
        var data = new FinishUserRegisterIn(token, password);

        var response = await http.PutAsJsonAsync("/users", data);

        var result = await response.Resolve<UserOut>();

        return result;
    }
}
