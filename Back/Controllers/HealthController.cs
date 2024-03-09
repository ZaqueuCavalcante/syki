namespace Syki.Back.Controllers;

[ApiController, Route("[controller]")]
[ApiExplorerSettings(IgnoreApi = true)]
public class HealthController : ControllerBase
{
    [HttpGet("")]
    public IActionResult GetHealth()
    {
        return Ok(new { Status = "Healthy" });
    }
}
