namespace Estud.Back.Features.Identity.GetSsoConfiguration;

[ApiController, Authorize(Policies.GetSsoConfiguration)]
public class GetSsoConfigurationController(GetSsoConfigurationService service) : ControllerBase
{
    /// <summary>
    /// Configuração SSO
    /// </summary>
    /// <remarks>
    /// Retorna a configuração SSO da instituição do usuário logado.
    /// </remarks>
    [HttpGet("identity/sso/configuration")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var result = await service.Get();
        return Ok(result);
    }
}

internal class ResponseExamples : ExamplesProvider<GetSsoConfigurationOut>;
