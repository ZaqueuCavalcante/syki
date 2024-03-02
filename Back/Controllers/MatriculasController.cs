using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Back.Controllers;

[EnableRateLimiting("Medium")]
[ApiController, Route("[controller]")]
public class MatriculasController : ControllerBase
{
    private readonly IMatriculasService _service;
    public MatriculasController(IMatriculasService service) => _service = service;

    [HttpPost()]
    [Authorize(Roles = Aluno)]
    public async Task<IActionResult> Create([FromBody] MatriculaTurmaIn data)
    {
        await _service.Create(User.Facul(), User.Id(), data);

        return Ok();
    }

    [HttpGet("turmas")]
    [Authorize(Roles = Aluno)]
    public async Task<IActionResult> GetTurmas()
    {
        var turmas = await _service.GetTurmas(User.Facul(), User.Id());

        return Ok(turmas);
    }
}
