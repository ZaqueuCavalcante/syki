namespace Syki.Back.Features.Identity.TwoFactorLogin;

[ApiController, EnableRateLimiting(RateLimitingConfigs.SensitivePolicy)]
public class TwoFactorLoginController(TwoFactorLoginService service) : ControllerBase
{
    /// <summary>
    /// 2FA Login
    /// </summary>
    /// <remarks>
    /// Realiza login utilizando o token 2FA.
    /// </remarks>
    [HttpPost("identity/2fa-login")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Login([FromBody] TwoFactorLoginIn data)
    {
        var result = await service.Login(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<TwoFactorLoginIn>;
internal class ResponseExamples : ExamplesProvider<TwoFactorLoginOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    InvalidTwoFactorToken
>;
