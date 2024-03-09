namespace Syki.Back.Controllers;

[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
public class TurmasController(ITurmasService service) : ControllerBase
{
    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] TurmaIn data)
    {
        var turma = await service.Create(User.InstitutionId(), data);

        return Ok(turma);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var turmas = await service.GetAll(User.InstitutionId());

        return Ok(turmas);
    }
}
