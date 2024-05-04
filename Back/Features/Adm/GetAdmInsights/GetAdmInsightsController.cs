namespace Syki.Back.GetAdmInsights;

[ApiController, AuthAdm]
public class GetAdmInsightsController(GetAdmInsightsService service) : ControllerBase
{
    [HttpGet("adm-insights")]
    public async Task<IActionResult> Get()
    {
        var data = await service.Get();
        
        return Ok(data);
    }
}
