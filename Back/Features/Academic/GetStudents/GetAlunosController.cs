namespace Syki.Back.GetAlunos;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class GetAlunosController(GetAlunosService service) : ControllerBase
{
    [HttpGet("alunos")]
    public async Task<IActionResult> Get()
    {
        var alunos = await service.Get(User.InstitutionId());
        
        return Ok(alunos);
    }
}
