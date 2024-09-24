namespace Syki.Back.Features.Academic.GetAcademicInsights;

/// <summary>
/// Retorna os Insights do Acadêmico.
/// </summary>
[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetAcademicInsightsController(GetAcademicInsightsService service) : ControllerBase
{
    [HttpGet("academic/insights")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Get()
    {
        var insights = await service.Get(User.InstitutionId());
        
        return Ok(insights);
    }
}
