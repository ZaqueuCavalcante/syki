namespace Syki.Back.Features.Teacher.GetTeacherAgenda;

[ApiController, AuthTeacher]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetTeacherAgendaController(GetTeacherAgendaService service) : ControllerBase
{
    /// <summary>
    /// Agenda
    /// </summary>
    /// <remarks>
    /// Retorna a agenda do professor.
    /// </remarks>
    [HttpGet("teacher/agenda")]
    public async Task<IActionResult> Get()
    {
        var data = await service.Get(User.InstitutionId(), User.Id());
        
        return Ok(data);
    }
}
