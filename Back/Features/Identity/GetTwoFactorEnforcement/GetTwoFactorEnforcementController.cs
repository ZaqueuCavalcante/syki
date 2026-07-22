namespace Estud.Back.Features.Identity.GetTwoFactorEnforcement;

[ApiController, Authorize(Policies.GetTwoFactorEnforcement)]
public class GetTwoFactorEnforcementController(GetTwoFactorEnforcementService service) : ControllerBase
{
    /// <summary>
    /// Obrigatoriedade de 2FA
    /// </summary>
    /// <remarks>
    /// Retorna os perfis de acesso da instituição indicando quais exigem autenticação de dois fatores.
    /// </remarks>
    [HttpGet("identity/2fa-enforcement")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var result = await service.Get();
        return Ok(result);
    }
}

internal class ResponseExamples : ExamplesProvider<GetTwoFactorEnforcementOut>;
