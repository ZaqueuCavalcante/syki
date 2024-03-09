namespace Syki.Back.Controllers;

[AuthAcademico]
[EnableRateLimiting("Medium")]
[ApiController, Route("[controller]")]
public class CursosController : ControllerBase
{
    private readonly ICursosService _service;
    public CursosController(ICursosService service) => _service = service;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] CursoIn data)
    {
        var curso = await _service.Create(User.InstitutionId(), data);

        return Ok(curso);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var cursos = await _service.GetAll(User.InstitutionId());

        return Ok(cursos);
    }

    [HttpGet("disciplinas")]
    public async Task<IActionResult> GetAllWithDisciplinas()
    {
        var cursos = await _service.GetAllWithDisciplinas(User.InstitutionId());

        return Ok(cursos);
    }

    [HttpGet("{id}/disciplinas")]
    public async Task<IActionResult> GetDisciplinas([FromRoute] Guid id)
    {
        var disciplinas = await _service.GetDisciplinas(id, User.InstitutionId());

        return Ok(disciplinas);
    }
}
