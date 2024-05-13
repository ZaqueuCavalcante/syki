namespace Syki.Back.Features.Student.GetStudentAgenda;

[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
public class GetStudentAgendaController(GetStudentAgendaService service) : ControllerBase
{
    [HttpGet("student/agenda")]
    public async Task<IActionResult> Get()
    {
        var agenda = await service.Get(User.InstitutionId(), User.Id());
        
        return Ok(agenda);
    }
}
