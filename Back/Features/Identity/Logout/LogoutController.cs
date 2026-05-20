using Syki.Back.Domain.Identity;

namespace Syki.Back.Features.Identity.Logout;

[ApiController, Authorize(Policies.Logout)]
public class LogoutController(SignInManager<SykiUser> signInManager, AuthSettings settings) : ControllerBase
{
    /// <summary>
    /// Logout
    /// </summary>
    /// <remarks>
    /// Realiza logout no sistema.
    /// </remarks>
    [HttpPost("identity/logout")]
    public async Task<IActionResult> Logout()
    {
        Response.DeleteJWTCookie(settings);
        await signInManager.SignOutAsync();

        return Ok();
    }
}
