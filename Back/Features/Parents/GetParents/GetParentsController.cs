namespace Estud.Back.Features.Parents.GetParents;

[ApiController, Authorize(Policies.GetParents)]
public class GetParentsController(GetParentsService service) : ControllerBase
{
    /// <summary>
    /// Responsáveis
    /// </summary>
    /// <remarks>
    /// Retorna a lista paginada de responsáveis da instituição, ordenados por nome.
    /// </remarks>
    [HttpGet("parents")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromQuery] GetParentsIn query)
    {
        var parents = await service.Get(query);
        return Ok(parents);
    }
}

internal class RequestExamples : ExamplesProvider<GetParentsIn>;
internal class ResponseExamples : ExamplesProvider<GetParentsOut>;
