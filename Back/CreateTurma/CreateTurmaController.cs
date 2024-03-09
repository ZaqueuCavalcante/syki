namespace Syki.Back.CreateTurma;

[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
public class CreateTurmaController(CreateTurmaService service) : ControllerBase
{
    [HttpPost("turmas")]
    public async Task<IActionResult> Create([FromBody] TurmaIn data)
    {
        var turma = await service.Create(User.InstitutionId(), data);

        return Ok(turma);
    }
}
