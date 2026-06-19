using Microsoft.AspNetCore.Mvc;

namespace Syki.Mocks.Oidc.Discovery;

/// <summary>
/// OpenID Connect Discovery Document.
/// Supports returning an oversized document for DoS testing.
/// </summary>
[ApiController]
public class DiscoveryController : ControllerBase
{
    [HttpGet("oidc/.well-known/openid-configuration")]
    public IActionResult Discovery()
    {
        return Ok();
    }
}
