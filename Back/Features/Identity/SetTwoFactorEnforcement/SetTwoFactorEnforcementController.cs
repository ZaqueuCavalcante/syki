namespace Estud.Back.Features.Identity.SetTwoFactorEnforcement;

[ApiController, Authorize(Policies.SetTwoFactorEnforcement)]
public class SetTwoFactorEnforcementController(SetTwoFactorEnforcementService service) : ControllerBase
{
    /// <summary>
    /// Definir obrigatoriedade de 2FA
    /// </summary>
    /// <remarks>
    /// Define se um perfil de acesso da instituição exige autenticação de dois fatores no login.
    /// </remarks>
    [HttpPut("identity/2fa-enforcement")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Set([FromBody] SetTwoFactorEnforcementIn data)
    {
        var result = await service.Set(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<SetTwoFactorEnforcementIn>;
internal class ResponseExamples : ExamplesProvider<SetTwoFactorEnforcementOut>;
internal class ErrorsExamples : ErrorExamplesProvider<RoleNotFound>;
