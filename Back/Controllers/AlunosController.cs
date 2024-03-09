namespace Syki.Back.Controllers;

[EnableRateLimiting("Medium")]
[ApiController, Route("[controller]")]
public class AlunosController(IAlunosService service) : ControllerBase
{
    [AuthAluno]
    [HttpGet("disciplinas")]
    public async Task<IActionResult> GetDisciplinas()
    {
        var disciplinas = await service.GetDisciplinas(User.Id());

        return Ok(disciplinas);
    }

    [HttpGet("")]
    [AuthAcademico]
    public async Task<IActionResult> GetAll()
    {
        var alunos = await service.GetAll(User.InstitutionId());
        
        return Ok(alunos);
    }
}
