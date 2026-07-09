using System.Net.Http.Json;
using Estud.Back.Features.Users.RegisterUser;
using Estud.Back.Features.Users.GetUserAccount;
using Estud.Back.Features.Users.UpdateUserAccount;

namespace Estud.Tests.Integration.Clients;

public partial class TestsHttpClient
{
    public async Task<OneOf<TestsUserDto, ErrorOut>> RegisterUser(string email)
    {
        var body = new RegisterUserIn { Email = email };

        var response = await http.PostAsJsonAsync("users/register", body);

        var result = await response.Resolve<TestsUserDto>();
        if (result.IsError) return result.Error;

        var user = result.Success;
        user.Email = email;

        return user;
    }

    public async Task<OneOf<GetUserAccountOut, ErrorOut>> GetUserAccount()
    {
        var response = await http.GetAsync("users/account");
        return await response.Resolve<GetUserAccountOut>();
    }

    public async Task<OneOf<SuccessOut, ErrorOut>> UpdateUserAccount(string name = "Edson Gomes")
    {
        var data = new UpdateUserAccountIn { Name = name };
        var response = await http.PutAsJsonAsync("users/account", data);
        return await response.Resolve<SuccessOut>();
    }
}
