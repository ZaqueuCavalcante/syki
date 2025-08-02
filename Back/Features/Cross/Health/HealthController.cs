namespace Syki.Back.Health;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class HealthController : ControllerBase
{
    /// <summary>
    /// Retorna o health status da API.
    /// </summary>
    [HttpGet("health")]
    public IActionResult Get()
    {
        return Ok(new { Status = "Healthy" });
    }
}
