namespace Syki.Back.Controllers;

[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
public class ProfessoresController(IProfessoresService service) : ControllerBase
{
    [HttpPost("professores")]
    public async Task<IActionResult> Create([FromBody] ProfessorIn data)
    {
        var professor = await service.Create(User.InstitutionId(), data);

        return Ok(professor);
    }

    [HttpGet("professores")]
    public async Task<IActionResult> GetAll()
    {
        var professores = await service.GetAll(User.InstitutionId());

        return Ok(professores);
    }
}
