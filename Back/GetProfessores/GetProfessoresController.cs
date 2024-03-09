namespace Syki.Back.GetProfessores;

[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
public class GetProfessoresController(GetProfessoresService service) : ControllerBase
{
    [HttpGet("professores")]
    public async Task<IActionResult> Get()
    {
        var professores = await service.Get(User.InstitutionId());

        return Ok(professores);
    }
}
