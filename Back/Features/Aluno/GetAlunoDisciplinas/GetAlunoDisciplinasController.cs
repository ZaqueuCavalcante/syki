namespace Syki.Back.GetAlunoDisciplinas;

[ApiController, AuthAluno]
[EnableRateLimiting("Medium")]
public class GetAlunoDisciplinasController(GetAlunoDisciplinasService service) : ControllerBase
{
    [HttpGet("alunos/disciplinas")]
    public async Task<IActionResult> Get()
    {
        var disciplinas = await service.Get(User.Id());

        return Ok(disciplinas);
    }
}
