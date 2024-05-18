namespace Syki.Back.Features.Adm.GetAdmInsights;

[ApiController, AuthAdm]
public class GetAdmInsightsController(GetAdmInsightsService service) : ControllerBase
{
    [HttpGet("adm/insights")]
    public async Task<IActionResult> Get()
    {
        var insights = await service.Get();
        
        return Ok(insights);
    }
}
