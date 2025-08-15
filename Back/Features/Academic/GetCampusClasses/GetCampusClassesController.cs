namespace Syki.Back.Features.Academic.GetCampusClasses;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class GetCampusClassesController(GetCampusClassesService service) : ControllerBase
{
    /// <summary>
    /// Turmas do campus
    /// </summary>
    /// <remarks>
    /// Retorna todas as turmas do campus especificado.
    /// </remarks>
    [HttpGet("academic/campi/{id}/classes")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var classes = await service.Get(User.InstitutionId, id);

        return Ok(classes);
    }
}
