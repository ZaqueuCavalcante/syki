namespace Syki.Back.Features.Academic.CreateDisciplina;

/// <summary>
/// Cria uma nova disciplina.
/// </summary>
[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateDisciplinaController(CreateDisciplinaService service) : ControllerBase
{
    [HttpPost("disciplinas")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Create([FromBody] DisciplinaIn data)
    {
        var disciplina = await service.Create(User.InstitutionId(), data);

        return Ok(disciplina);
    }
}
