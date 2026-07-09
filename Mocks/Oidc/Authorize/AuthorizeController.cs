using Microsoft.AspNetCore.Mvc;

namespace Estud.Mocks.Oidc.Authorize;

/// <summary>
/// Authorization endpoint - initiates the login flow.
/// Simulates a successful IdP authentication by immediately redirecting back with an authorization code.
/// Uses login_hint to resolve per-challenge config, eliminating global state races in parallel tests.
/// </summary>
[ApiController]
public class AuthorizeController : ControllerBase
{
    [HttpGet("oidc/connect/authorize")]
    public IActionResult Authorize()
    {
        return Redirect("");
    }
}
