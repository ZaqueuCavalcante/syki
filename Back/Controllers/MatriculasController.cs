using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Back.Controllers;

[EnableRateLimiting("Medium")]
[ApiController, Route("[controller]")]
public class MatriculasController(IMatriculasService service) : ControllerBase
{
    [AuthAluno]
    [HttpPost()]
    public async Task<IActionResult> Create([FromBody] MatriculaTurmaIn data)
    {
        await service.Create(User.Facul(), User.Id(), data);

        return Ok();
    }

    [AuthAluno]
    [HttpGet("turmas")]
    public async Task<IActionResult> GetTurmas()
    {
        var turmas = await service.GetTurmas(User.Facul(), User.Id());

        return Ok(turmas);
    }
}
