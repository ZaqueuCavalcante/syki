namespace Syki.Back.Features.Identity.GoogleOneTapLogin;

[ApiController, EnableRateLimiting(RateLimitingConfigs.SensitivePolicy)]
public class GoogleOneTapLoginController(GoogleOneTapLoginService service) : ControllerBase
{
    /// <summary>
    /// Google One Tap Login 🔓
    /// </summary>
    /// <remarks>
    /// Realiza login via Google One Tap (ID token).
    /// </remarks>
    [HttpPost("identity/social-login/google-one-tap")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Login([FromBody] GoogleOneTapLoginIn data)
    {
        var result = await service.Login(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<GoogleOneTapLoginIn>;
internal class ResponseExamples : ExamplesProvider<GoogleOneTapLoginOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    GoogleOneTapLoginInvalidToken,
    GoogleOneTapLoginDisabled,
    SocialLoginEmailNotVerified,
    SocialLoginSsoRequired
>;
