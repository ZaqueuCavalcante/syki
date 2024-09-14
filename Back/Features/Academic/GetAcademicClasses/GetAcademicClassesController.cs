namespace Syki.Back.Features.Academic.GetAcademicClasses;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetAcademicClassesController(GetAcademicClassesService service) : ControllerBase
{
    [HttpGet("academic/classes")]
    public async Task<IActionResult> Get([FromQuery] GetAcademicClassesIn query)
    {
        var classes = await service.Get(User.InstitutionId(), query);

        return Ok(classes);
    }
}
