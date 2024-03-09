namespace Syki.Back.Controllers;

[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
public class GradesController(IGradesService service) : ControllerBase
{
    [HttpGet("grades/{id}/disciplinas")]
    public async Task<IActionResult> GetDisciplinas([FromRoute] Guid id)
    {
        var grades = await service.GetDisciplinas(User.InstitutionId(), id);

        return Ok(grades);
    }
}
