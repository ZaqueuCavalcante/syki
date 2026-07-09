namespace Estud.Back.Features.Disciplines.GetDisciplines;

[ApiController, Authorize(Policies.GetDisciplines)]
public class GetDisciplinesController(GetDisciplinesService service) : ControllerBase
{
    /// <summary>
    /// Disciplinas
    /// </summary>
    /// <remarks>
    /// Retorna todas as disciplinas.
    /// </remarks>
    [HttpGet("disciplines")]
    public async Task<IActionResult> Get([FromQuery] GetDisciplinesIn query)
    {
        var disciplines = await service.Get(query);
        return Ok(disciplines);
    }
}

internal class RequestExamples : ExamplesProvider<GetDisciplinesIn>;
