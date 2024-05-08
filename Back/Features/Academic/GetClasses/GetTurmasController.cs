namespace Syki.Back.GetTurmas;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class GetTurmasController(GetTurmasService service) : ControllerBase
{
    [HttpGet("turmas")]
    public async Task<IActionResult> Get()
    {
        var turmas = await service.Get(User.InstitutionId());

        return Ok(turmas);
    }
}
