using System.Net.Http.Json;
using Syki.Back.Features.Identity.MagicLinkLogin;

namespace Syki.Tests.Integration.Clients;

public partial class TestsHttpClient
{
    public async Task<OneOf<MagicLinkLoginOut, ErrorOut>> MagicLinkLogin(string token)
    {
        var data = new MagicLinkLoginIn { Token = token };

        var response = await http.PostAsJsonAsync("identity/magic-link-login", data);

        return await response.Resolve<MagicLinkLoginOut>();
    }
}
