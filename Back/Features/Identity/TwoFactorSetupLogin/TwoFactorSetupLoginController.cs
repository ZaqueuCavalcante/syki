namespace Estud.Back.Features.Identity.TwoFactorSetupLogin;

[ApiController, Authorize(Policies.TwoFactorSetupLogin), EnableRateLimiting(RateLimitingConfigs.SensitivePolicy)]
public class TwoFactorSetupLoginController(TwoFactorSetupLoginService service) : ControllerBase
{
    /// <summary>
    /// Login após setup de 2FA
    /// </summary>
    /// <remarks>
    /// Conclui o login do fluxo de 2FA obrigatório: troca a credencial de setup pelo JWT com as permissões reais do usuário, desde que o 2FA já tenha sido habilitado.
    /// </remarks>
    [HttpPost("identity/2fa-setup-login")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Login()
    {
        var result = await service.Login();
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class ResponseExamples : ExamplesProvider<TwoFactorSetupLoginOut>;
internal class ErrorsExamples : ErrorExamplesProvider<TwoFactorSetupNotCompleted>;
