namespace Syki.Back.Features.Teacher.GetTeacherAgenda;

[ApiController, AuthTeacher]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetTeacherAgendaController(GetTeacherAgendaService service) : ControllerBase
{
    [HttpGet("teacher/agenda")]
    public async Task<IActionResult> Get()
    {
        var data = await service.Get(User.InstitutionId(), User.Id());
        
        return Ok(data);
    }
}
