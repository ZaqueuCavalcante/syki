namespace Syki.Back.GetAlunoAgenda;

[ApiController, AuthAluno]
[EnableRateLimiting("Medium")]
public class GetAlunoAgendaController(GetAlunoAgendaService service) : ControllerBase
{
    [HttpGet("aluno-agenda")]
    public async Task<IActionResult> Get()
    {
        var data = await service.Get(User.InstitutionId(), User.Id());
        
        return Ok(data);
    }
}
