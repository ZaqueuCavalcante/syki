namespace Syki.Back.GetGrades;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class GetGradesController(GetGradesService service) : ControllerBase
{
    [HttpGet("grades")]
    public async Task<IActionResult> Get()
    {
        var grades = await service.Get(User.InstitutionId());

        return Ok(grades);
    }
}
