using System.Net.Http.Json;
using Syki.Back.Features.Users.RegisterUser;

namespace Syki.Tests.Integration.Clients;

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
}
