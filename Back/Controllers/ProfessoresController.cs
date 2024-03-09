namespace Syki.Back.Controllers;

[AuthAcademico]
[EnableRateLimiting("Medium")]
[ApiController, Route("[controller]")]
public class ProfessoresController : ControllerBase
{
    private readonly IProfessoresService _service;
    public ProfessoresController(IProfessoresService service) => _service = service;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] ProfessorIn data)
    {
        var professor = await _service.Create(User.InstitutionId(), data);

        return Ok(professor);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var professores = await _service.GetAll(User.InstitutionId());

        return Ok(professores);
    }
}
