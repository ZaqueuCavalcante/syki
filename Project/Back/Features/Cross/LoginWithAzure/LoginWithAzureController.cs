using Microsoft.AspNetCore.Authentication;
using Exato.Back.Features.Cross.CreateExatoUser;

namespace Exato.Back.Features.Cross.Login;

[ApiController]
public class LoginWithAzureController(SignInManager<ExatoUser> signInManager) : ControllerBase
{
    /// <summary>
    /// Login com Azure 🔓
    /// </summary>
    [HttpGet("login/azure")]
    public async Task LoginWithAzure()
    {
        Response.Cookies.Delete(AuthenticationConfigs.BearerCookie);
        HttpContext.ClearOidcStateCookies();
        await signInManager.SignOutAsync();

        var properties = new AuthenticationProperties { RedirectUri = "/" };
        await HttpContext.ChallengeAsync(AuthenticationConfigs.AzureOIDCScheme, properties);
    }
}
