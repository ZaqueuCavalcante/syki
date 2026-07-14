namespace Estud.Back.Features.Institutions.GetInstitutionConfig;

[ApiController, Authorize(Policies.GetInstitutionConfig)]
public class GetInstitutionConfigController(GetInstitutionConfigService service) : ControllerBase
{
    /// <summary>
    /// Configurações da instituição
    /// </summary>
    /// <remarks>
    /// Retorna a nota e a frequência mínimas para aprovação nas disciplinas da instituição do usuário logado.
    /// </remarks>
    [HttpGet("institutions/config")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var config = await service.Get();
        return Ok(config);
    }
}

internal class ResponseExamples : ExamplesProvider<GetInstitutionConfigOut>;
