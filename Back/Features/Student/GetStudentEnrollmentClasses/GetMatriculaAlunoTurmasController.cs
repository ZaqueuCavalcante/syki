namespace Syki.Back.GetMatriculaAlunoTurmas;

[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
public class GetMatriculaAlunoTurmasController(GetMatriculaAlunoTurmasService service) : ControllerBase
{
    [HttpGet("matriculas/aluno/turmas")]
    public async Task<IActionResult> Get()
    {
        var turmas = await service.Get(User.InstitutionId(), User.Id());

        return Ok(turmas);
    }
}
