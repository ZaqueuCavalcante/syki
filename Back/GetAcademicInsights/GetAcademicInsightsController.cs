namespace Syki.Back.GetAcademicInsights;

[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
public class GetAcademicInsightsController(GetAcademicInsightsService service) : ControllerBase
{
    [HttpGet("academic-insights")]
    public async Task<IActionResult> Get()
    {
        var data = await service.Get(User.InstitutionId());
        
        return Ok(data);
    }
}
