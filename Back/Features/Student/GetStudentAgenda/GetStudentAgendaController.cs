namespace Syki.Back.Features.Student.GetStudentAgenda;

/// <summary>
/// Retorna a Agenda do Aluno.
/// </summary>
[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetStudentAgendaController(GetStudentAgendaService service) : ControllerBase
{
    [HttpGet("student/agenda")]
    public async Task<IActionResult> Get()
    {
        var agenda = await service.Get(User.InstitutionId(), User.Id());
        
        return Ok(agenda);
    }
}
