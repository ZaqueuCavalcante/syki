using Microsoft.AspNetCore.Mvc;

namespace Syki.Mocks.SocialLogin.Google.Token;

/// <summary>
/// Mock Google OAuth token endpoint.
/// Exchanges authorization code for access token.
/// </summary>
[ApiController]
public class GoogleTokenController : ControllerBase
{
    [HttpPost("social-login/google/token")]
    public IActionResult Token()
    {
        return Ok();
    }
}
