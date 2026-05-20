namespace Syki.Back.Features.Disciplines.GetDisciplines;

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
    public async Task<IActionResult> Get()
    {
        var disciplines = await service.Get();
        return Ok(disciplines);
    }
}
