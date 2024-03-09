namespace Syki.Back.Controllers;

[ApiController]
[EnableRateLimiting("Medium")]
public class AlunosController(IAlunosService service) : ControllerBase
{
    [AuthAluno]
    [HttpGet("disciplinas")]
    public async Task<IActionResult> GetDisciplinas()
    {
        var disciplinas = await service.GetDisciplinas(User.Id());

        return Ok(disciplinas);
    }
}
