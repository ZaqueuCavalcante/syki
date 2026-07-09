using Microsoft.AspNetCore.Mvc;

namespace Estud.Mocks.Oidc.Token;

/// <summary>
/// Token endpoint - exchanges authorization code for tokens.
/// Uses per-auth-code captured state for parallel test safety.
/// Supports producing malicious tokens for security testing.
/// </summary>
[ApiController]
public class TokenController : ControllerBase
{
    [HttpPost("oidc/connect/token")]
    public IActionResult Token()
    {
        return Ok();
    }
}
