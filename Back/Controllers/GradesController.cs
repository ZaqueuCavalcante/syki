namespace Syki.Back.Controllers;

[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
public class GradesController(IGradesService service) : ControllerBase
{
    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] GradeIn data)
    {
        var grade = await service.Create(User.InstitutionId(), data);

        return Ok(grade);
    }

    [HttpGet("{id}/disciplinas")]
    public async Task<IActionResult> GetDisciplinas([FromRoute] Guid id)
    {
        var grades = await service.GetDisciplinas(User.InstitutionId(), id);

        return Ok(grades);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var grades = await service.GetAll(User.InstitutionId());

        return Ok(grades);
    }
}
