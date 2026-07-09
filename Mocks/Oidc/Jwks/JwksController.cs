using Microsoft.AspNetCore.Mvc;

namespace Estud.Mocks.Oidc.Jwks;

/// <summary>
/// JSON Web Key Set - public keys for token signature validation.
/// Supports returning many fake keys for DoS testing.
/// </summary>
[ApiController]
public class JwksController : ControllerBase
{
    [HttpGet("oidc/.well-known/jwks")]
    public IActionResult Jwks()
    {
        return Ok();
    }
}
