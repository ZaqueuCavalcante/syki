using Microsoft.AspNetCore.Mvc;

namespace Estud.Mocks.SocialLogin.Google.Authorize;

/// <summary>
/// Mock Google OAuth authorization endpoint.
/// Simulates Google's /o/oauth2/v2/auth by immediately redirecting with an authorization code.
/// Uses login_hint to resolve per-challenge config for parallel test safety.
/// </summary>
[ApiController]
public class GoogleAuthorizeController : ControllerBase
{
    [HttpGet("social-login/google/authorize")]
    public IActionResult Authorize()
    {
        return Redirect("");
    }
}
