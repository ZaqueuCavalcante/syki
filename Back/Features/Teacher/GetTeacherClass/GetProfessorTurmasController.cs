namespace Syki.Back.GetProfessorTurma;

[ApiController, AuthTeacher]
[EnableRateLimiting("Medium")]
public class GetProfessorTurmaController(GetProfessorTurmaService service) : ControllerBase
{
    [HttpGet("professor-turma")]
    public async Task<IActionResult> Get([FromQuery] string turmaId)
    {
        var turma = await service.Get(User.InstitutionId(), User.Id(), turmaId);

        return Ok(turma);
    }
}
