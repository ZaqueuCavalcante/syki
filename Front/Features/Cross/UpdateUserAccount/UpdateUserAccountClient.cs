namespace Syki.Front.Features.Cross.UpdateUserAccount;

public class UpdateUserAccountClient(HttpClient http) : ICrossClient
{
    public async Task<OneOf<SuccessOut, ErrorOut>> Update(string name, string? profilePhoto)
    {
        var data = new UpdateUserAccountIn { Name = name, ProfilePhoto = profilePhoto };

        var response = await http.PutAsJsonAsync("/user/account", data);

        var result = await response.Resolve<SuccessOut>();

        return result;
    }
}
