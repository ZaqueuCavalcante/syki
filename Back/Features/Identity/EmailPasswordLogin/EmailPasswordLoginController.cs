namespace Syki.Back.Features.Identity.EmailPasswordLogin;

[ApiController, EnableRateLimiting(RateLimitingConfigs.SensitivePolicy)]
public class EmailPasswordLoginController(EmailPasswordLoginService service) : ControllerBase
{
    /// <summary>
    /// Email + Password Login 🔓
    /// </summary>
    /// <remarks>
    /// Realiza login no sistema.
    /// </remarks>
    [HttpPost("identity/email-password-login")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Login([FromBody] EmailPasswordLoginIn data)
    {
        var result = await service.Login(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<EmailPasswordLoginIn>;
internal class ResponseExamples : ExamplesProvider<EmailPasswordLoginOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    LoginRequiresTwoFactor,
    LoginUserLockedOut,
    LoginWrongEmailOrPassword
>;
