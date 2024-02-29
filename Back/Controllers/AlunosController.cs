using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Back.Controllers;

[EnableRateLimiting("Medium")]
[ApiController, Route("[controller]")]
public class AlunosController : ControllerBase
{
    private readonly IAlunosService _service;
    public AlunosController(IAlunosService service) => _service = service;

    [HttpGet("disciplinas")]
    [Authorize(Roles = Aluno)]
    public async Task<IActionResult> GetDisciplinas()
    {
        var disciplinas = await _service.GetDisciplinas(User.Id());

        return Ok(disciplinas);
    }

    [HttpGet("")]
    [Authorize(Roles = Academico)]
    public async Task<IActionResult> GetAll()
    {
        var alunos = await _service.GetAll(User.Facul());
        
        return Ok(alunos);
    }
}
