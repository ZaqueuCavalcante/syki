namespace Syki.Back.GetProfessorAgenda;

[ApiController, AuthTeacher]
[EnableRateLimiting("Medium")]
public class GetProfessorAgendaController(GetProfessorAgendaService service) : ControllerBase
{
    [HttpGet("professor-agenda")]
    public async Task<IActionResult> Get()
    {
        var data = await service.Get(User.InstitutionId(), User.Id());
        
        return Ok(data);
    }
}
