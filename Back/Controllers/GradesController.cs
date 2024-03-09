namespace Syki.Back.Controllers;

[AuthAcademico]
[EnableRateLimiting("Medium")]
[ApiController, Route("[controller]")]
public class GradesController : ControllerBase
{
    private readonly IGradesService _service;
    public GradesController(IGradesService service) => _service = service;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] GradeIn data)
    {
        var grade = await _service.Create(User.InstitutionId(), data);

        return Ok(grade);
    }

    [HttpGet("{id}/disciplinas")]
    public async Task<IActionResult> GetDisciplinas([FromRoute] Guid id)
    {
        var grades = await _service.GetDisciplinas(User.InstitutionId(), id);

        return Ok(grades);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var grades = await _service.GetAll(User.InstitutionId());

        return Ok(grades);
    }
}
