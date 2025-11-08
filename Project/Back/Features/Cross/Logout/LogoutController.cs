using Exato.Back.Features.Cross.CreateExatoUser;

namespace Exato.Back.Features.Cross.Logout;

[ApiController]
public class LogoutController(SignInManager<ExatoUser> signInManager) : ControllerBase
{
    /// <summary>
    /// Logout 🔓
    /// </summary>
    /// <remarks>
    /// Realiza Logout no sistema.
    /// </remarks>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        Response.Cookies.Delete(AuthenticationConfigs.BearerCookie);
        HttpContext.ClearOidcStateCookies();
        await signInManager.SignOutAsync();

        return Ok();
    }
}
