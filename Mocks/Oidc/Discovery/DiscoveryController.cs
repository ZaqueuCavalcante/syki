using Microsoft.AspNetCore.Mvc;

namespace Estud.Mocks.Oidc.Discovery;

/// <summary>
/// OpenID Connect Discovery Document.
/// </summary>
[ApiController]
public class DiscoveryController : ControllerBase
{
    [HttpGet("oidc/.well-known/openid-configuration")]
    public IActionResult Discovery()
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";

        return Ok(new
        {
            issuer = baseUrl,
            authorization_endpoint = $"{baseUrl}/connect/authorize",
            token_endpoint = $"{baseUrl}/connect/token",
            userinfo_endpoint = $"{baseUrl}/connect/userinfo",
            jwks_uri = $"{baseUrl}/.well-known/jwks",
            response_types_supported = new[] { "code" },
            subject_types_supported = new[] { "public" },
            id_token_signing_alg_values_supported = new[] { "RS256" },
            scopes_supported = new[] { "openid", "email", "profile" },
            token_endpoint_auth_methods_supported = new[] { "client_secret_post" },
            claims_supported = new[] { "sub", "email", "email_verified", "name" },
        });
    }
}
