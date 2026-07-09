using Microsoft.AspNetCore.Mvc;

namespace Estud.Mocks.SocialLogin.Google.UserInfo;

/// <summary>
/// Mock Google OAuth UserInfo endpoint.
/// Returns user claims based on the access token session state.
/// </summary>
[ApiController]
public class GoogleUserInfoController : ControllerBase
{
    [HttpGet("social-login/google/userinfo")]
    public IActionResult UserInfo()
    {
        return Ok();
    }
}
