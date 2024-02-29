namespace Syki.Back.CreateAluno;

[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
public class CreateAlunoController(CreateAlunoService service) : ControllerBase
{
    [HttpPost("alunos")]
    public async Task<IActionResult> Create([FromBody] AlunoIn data)
    {
        var aluno = await service.Create(User.InstitutionId(), data);

        return Ok(aluno);
    }
}
