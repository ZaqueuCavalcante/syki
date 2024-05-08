namespace Syki.Back.CreateProfessor;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class CreateProfessorController(CreateProfessorService service) : ControllerBase
{
    [HttpPost("professores")]
    public async Task<IActionResult> Create([FromBody] ProfessorIn data)
    {
        var professor = await service.Create(User.InstitutionId(), data);

        return Ok(professor);
    }
}
