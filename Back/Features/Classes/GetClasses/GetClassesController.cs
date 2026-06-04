namespace Syki.Back.Features.Classes.GetClasses;

[ApiController, Authorize(Policies.GetClasses)]
public class GetClassesController(GetClassesService service) : ControllerBase
{
    /// <summary>
    /// Turmas
    /// </summary>
    /// <remarks>
    /// Retorna todas as turmas da instituição do usuário logado.
    /// </remarks>
    [HttpGet("classes")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromQuery] GetAcademicClassesIn query)
    {
        var classes = await service.Get(query);
        return Ok(classes);
    }
}

internal class RequestExamples : ExamplesProvider<GetAcademicClassesIn>;
internal class ResponseExamples : ExamplesProvider<GetClassesOut>;
