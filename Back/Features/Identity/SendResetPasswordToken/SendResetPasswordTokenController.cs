namespace Estud.Back.Features.Identity.SendResetPasswordToken;

[ApiController, EnableRateLimiting(RateLimitingConfigs.SensitivePolicy)]
public class SendResetPasswordTokenController(SendResetPasswordTokenService service) : ControllerBase
{
    /// <summary>
    /// Esqueci minha senha 🔓
    /// </summary>
    /// <remarks>
    /// Envia um link de redefinição de senha para o email informado.
    /// </remarks>
    [HttpPost("identity/reset-password-token")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Send([FromBody] SendResetPasswordTokenIn data)
    {
        var result = await service.Send(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<SendResetPasswordTokenIn>;
internal class ResponseExamples : ExamplesProvider<SuccessOut>;
