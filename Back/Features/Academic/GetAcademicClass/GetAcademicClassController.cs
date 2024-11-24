namespace Syki.Back.Features.Academic.GetAcademicClass;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetAcademicClassController(GetAcademicClassService service) : ControllerBase
{
    /// <summary>
    /// Turma
    /// </summary>
    /// <remarks>
    /// Retorna a turma informada.
    /// </remarks>
    [HttpGet("academic/classes/{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await service.Get(User.InstitutionId(), id);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
