namespace Syki.Back.CreateMatriculaAluno;

[ApiController, AuthAluno]
[EnableRateLimiting("Medium")]
public class CreateMatriculaAlunoController(CreateMatriculaAlunoService service) : ControllerBase
{
    [HttpPost("matriculas/aluno")]
    public async Task<IActionResult> Create([FromBody] MatriculaTurmaIn data)
    {
        await service.Create(User.InstitutionId(), User.Id(), data);

        return Ok();
    }
}
