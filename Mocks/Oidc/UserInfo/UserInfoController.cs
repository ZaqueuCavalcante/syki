using Microsoft.AspNetCore.Mvc;

namespace Estud.Mocks.Oidc.UserInfo;

/// <summary>
/// UserInfo endpoint - returns authenticated user claims.
/// Uses per-access-token session state for parallel test safety.
/// </summary>
[ApiController]
public class UserInfoController : ControllerBase
{
    [HttpGet("oidc/connect/userinfo")]
    public IActionResult UserInfo()
    {
        return Ok();
    }
}
