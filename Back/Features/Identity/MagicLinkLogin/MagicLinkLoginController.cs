namespace Estud.Back.Features.Identity.MagicLinkLogin;

[ApiController, EnableRateLimiting(RateLimitingConfigs.SensitivePolicy)]
public class MagicLinkLoginController(MagicLinkLoginService service) : ControllerBase
{
    /// <summary>
    /// Magic Link Login 🔓
    /// </summary>
    /// <remarks>
    /// Realiza login via magic link.
    /// </remarks>
    [HttpPost("identity/magic-link-login")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Login([FromBody] MagicLinkLoginIn data)
    {
        var result = await service.Login(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<MagicLinkLoginIn>;
internal class ResponseExamples : ExamplesProvider<MagicLinkLoginOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    InvalidMagicLink
>;
