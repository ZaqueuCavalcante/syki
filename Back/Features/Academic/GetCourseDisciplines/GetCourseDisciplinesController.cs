namespace Syki.Back.Features.Academic.GetCourseDisciplines;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class GetCourseDisciplinesController(GetCourseDisciplinesService service) : ControllerBase
{
    /// <summary>
    /// Disciplinas do curso
    /// </summary>
    /// <remarks>
    /// Retorna todas as disciplinas do curso informado.
    /// </remarks>
    [HttpGet("academic/courses/{id}/disciplines")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var disciplines = await service.Get(id, User.InstitutionId());
        return Ok(disciplines);
    }
}
