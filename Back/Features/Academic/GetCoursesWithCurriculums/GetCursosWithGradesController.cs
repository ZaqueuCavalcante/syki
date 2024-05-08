namespace Syki.Back.GetCursosWithGrades;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class GetCursosWithGradesController(GetCursosWithGradesService service) : ControllerBase
{
    [HttpGet("cursos/with-grades")]
    public async Task<IActionResult> Get()
    {
        var cursos = await service.Get(User.InstitutionId());

        return Ok(cursos);
    }
}
