namespace Syki.Back.CreateTurma;

/// <summary>
/// Cria uma nova turma.
/// </summary>
[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateTurmaController(CreateTurmaService service) : ControllerBase
{
    [HttpPost("turmas")]
    [ProducesResponseType(typeof(TurmaOut), 200)]
    public async Task<IActionResult> Create([FromBody] TurmaIn data)
    {
        var turma = await service.Create(User.InstitutionId(), data);

        return Ok(turma);
    }
}
