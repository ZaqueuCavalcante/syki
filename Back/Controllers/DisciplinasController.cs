namespace Syki.Back.Controllers;

[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
public class DisciplinasController(IDisciplinasService service) : ControllerBase
{
    [HttpPost()]
    public async Task<IActionResult> Create([FromBody] DisciplinaIn data)
    {
        var disciplina = await service.Create(User.InstitutionId(), data);

        return Ok(disciplina);
    }

    [HttpGet()]
    public async Task<IActionResult> GetAll([FromQuery] Guid? cursoId)
    {
        var disciplinas = await service.GetAll(User.InstitutionId(), cursoId);

        return Ok(disciplinas);
    }
}
