namespace Estud.Back.Features.Institutions.SetupInstitutionConfig;

[ApiController, Authorize(Policies.SetupInstitutionConfig)]
public class SetupInstitutionConfigController(SetupInstitutionConfigService service) : ControllerBase
{
    /// <summary>
    /// Configurar instituição
    /// </summary>
    /// <remarks>
    /// Define a nota e a frequência mínimas para aprovação nas disciplinas da instituição.
    /// </remarks>
    [HttpPost("institutions/config")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Setup([FromBody] SetupInstitutionConfigIn data)
    {
        var result = await service.Setup(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<SetupInstitutionConfigIn>;
internal class ResponseExamples : ExamplesProvider<SetupInstitutionConfigOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    InvalidNoteLimit,
    InvalidFrequencyLimit
>;
