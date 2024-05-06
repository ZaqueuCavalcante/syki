namespace Syki.Back.GetProfessorTurmas;

[ApiController, AuthProfessor]
[EnableRateLimiting("Medium")]
public class GetProfessorTurmasController(GetProfessorTurmasService service) : ControllerBase
{
    [HttpGet("professor-turmas")]
    public async Task<IActionResult> Get()
    {
        var turmas = await service.Get(User.InstitutionId(), User.Id());

        return Ok(turmas);
    }
}
