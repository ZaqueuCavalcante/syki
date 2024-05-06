namespace Syki.Back.Features.Academic.GetAcademicInsights;

/// <summary>
/// Retorna os insights do usuário do tipo acadêmico.
/// </summary>
[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetAcademicInsightsController(GetAcademicInsightsService service) : ControllerBase
{
    [HttpGet("academic-insights")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Get()
    {
        var data = await service.Get(User.InstitutionId());
        
        return Ok(data);
    }
}
