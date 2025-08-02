namespace Syki.Back.Features.Student.GetStudentAgenda;

[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
public class GetStudentAgendaController(GetStudentAgendaService service) : ControllerBase
{
    /// <summary>
    /// Agenda
    /// </summary>
    /// <remarks>
    /// Retorna a agenda do aluno.
    /// </remarks>
    [HttpGet("student/agenda")]
    public async Task<IActionResult> Get()
    {
        var agenda = await service.Get(User.InstitutionId(), User.Id());
        
        return Ok(agenda);
    }
}
