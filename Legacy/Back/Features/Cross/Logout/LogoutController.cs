using Syki.Back.Features.Cross.CreateUser;

namespace Syki.Back.Features.Cross.Logout;

[ApiController]
[EnableRateLimiting("Small")]
public class LogoutController(SignInManager<SykiUser> signInManager) : ControllerBase
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
        Response.Cookies.Delete("syki_jwt");
        await signInManager.SignOutAsync();
        return Ok();
    }
}
