namespace Syki.Back.Features.Academic.GetCourseDisciplines;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetCourseDisciplinesController(GetCourseDisciplinesService service) : ControllerBase
{
    /// <summary>
    /// Disciplinas
    /// </summary>
    /// <remarks>
    /// Retorna todas as disciplinas.
    /// </remarks>
    [HttpGet("academic/courses/{id}/disciplines")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var disciplines = await service.Get(id, User.InstitutionId());

        return Ok(disciplines);
    }
}
