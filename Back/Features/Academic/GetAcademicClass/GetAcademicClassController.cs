namespace Syki.Back.Features.Academic.GetAcademicClass;

/// <summary>
/// Retorna uma Turma, dado seu id.
/// </summary>
[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetAcademicClassController(GetAcademicClassService service) : ControllerBase
{
    [HttpGet("academic/classes/{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await service.Get(User.InstitutionId(), id);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
