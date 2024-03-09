namespace Syki.Back.CreateDisciplina;

[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
public class CreateDisciplinaController(CreateDisciplinaService service) : ControllerBase
{
    [HttpPost("disciplinas")]
    public async Task<IActionResult> Create([FromBody] DisciplinaIn data)
    {
        var disciplina = await service.Create(User.InstitutionId(), data);

        return Ok(disciplina);
    }
}
