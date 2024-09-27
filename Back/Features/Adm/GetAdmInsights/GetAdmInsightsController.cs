namespace Syki.Back.Features.Adm.GetAdmInsights;

/// <summary>
/// Retorna os Insights do Admin.
/// </summary>
[ApiController, AuthAdm]
[Consumes("application/json"), Produces("application/json")]
public class GetAdmInsightsController(GetAdmInsightsService service) : ControllerBase
{
    [HttpGet("adm/insights")]
    public async Task<IActionResult> Get()
    {
        var insights = await service.Get();
        
        return Ok(insights);
    }
}
