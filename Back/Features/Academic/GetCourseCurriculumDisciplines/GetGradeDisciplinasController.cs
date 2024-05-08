namespace Syki.Back.GetGradeDisciplinas;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class GetGradeDisciplinasController(GetGradeDisciplinasService service) : ControllerBase
{
    [HttpGet("grades/{id}/disciplinas")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var grades = await service.Get(User.InstitutionId(), id);

        return Ok(grades);
    }
}
