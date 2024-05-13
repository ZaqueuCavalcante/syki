namespace Syki.Back.Features.Academic.GetClasses;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class GetClassesController(GetClassesService service) : ControllerBase
{
    [HttpGet("academic/classes")]
    public async Task<IActionResult> Get()
    {
        var classes = await service.Get(User.InstitutionId());

        return Ok(classes);
    }
}
