namespace Syki.Back.Features.Academic.GetAcademicClasses;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class GetAcademicClassesController(GetAcademicClassesService service) : ControllerBase
{
    /// <summary>
    /// Turmas
    /// </summary>
    /// <remarks>
    /// Retorna todas as turmas da instituição.
    /// </remarks>
    [HttpGet("academic/classes")]
    public async Task<IActionResult> Get([FromQuery] GetAcademicClassesIn query)
    {
        var classes = await service.Get(User.InstitutionId(), query);

        return Ok(classes);
    }
}
