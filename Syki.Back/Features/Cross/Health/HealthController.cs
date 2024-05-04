namespace Syki.Back.Health;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class HealthController : ControllerBase
{
    [HttpGet("health")]
    public IActionResult Get()
    {
        return Ok(new { Status = "Healthy" });
    }
}
