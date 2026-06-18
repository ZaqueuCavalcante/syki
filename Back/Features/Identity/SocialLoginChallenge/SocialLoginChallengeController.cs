using Syki.Back.Auth.Schemes;
using Microsoft.AspNetCore.Authentication;

namespace Syki.Back.Features.Identity.SocialLoginChallenge;

[ApiController, EnableRateLimiting(RateLimitingConfigs.SensitivePolicy)]
public class SocialLoginChallengeController(FrontendSettings frontendSettings) : ControllerBase
{
    /// <summary>
    /// Social Login Challenge 🔓
    /// </summary>
    /// <remarks>
    /// Redirects to the social login provider (Google) for authentication.
    /// This is a browser redirect endpoint, not a JSON API.
    /// The optional email parameter sets login_hint for the provider.
    /// </remarks>
    [HttpGet("identity/social-login/challenge/{provider}")]
    public IActionResult Challenge(string provider, [FromQuery] string? email = null)
    {
        Enum.TryParse(provider, ignoreCase: true, out SocialLoginProvider loginProvider);

        var schemeName = loginProvider switch
        {
            SocialLoginProvider.Google => SocialLoginScheme.GoogleScheme,
            _ => null,
        };

        if (schemeName == null) return Redirect($"{frontendSettings.Url}?social_login_error={nameof(SocialLoginFailed)}");

        var properties = new AuthenticationProperties
        {
            RedirectUri = "/home",
        };

        if (email != null) properties.Items["login_hint"] = email;

        return Challenge(properties, schemeName);
    }
}
