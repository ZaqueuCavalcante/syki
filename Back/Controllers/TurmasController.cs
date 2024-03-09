namespace Syki.Back.Controllers;

[AuthAcademico]
[EnableRateLimiting("Medium")]
[ApiController, Route("[controller]")]
public class TurmasController : ControllerBase
{
    private readonly ITurmasService _service;
    public TurmasController(ITurmasService service) => _service = service;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] TurmaIn data)
    {
        var turma = await _service.Create(User.Facul(), data);

        return Ok(turma);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var turmas = await _service.GetAll(User.Facul());

        return Ok(turmas);
    }
}
