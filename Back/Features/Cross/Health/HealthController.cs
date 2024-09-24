namespace Syki.Back.Health;

/// <summary>
/// Retorna o Status da API.
/// </summary>
[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
[Consumes("application/json"), Produces("application/json")]
public class HealthController : ControllerBase
{
    [HttpGet("health")]
    public IActionResult Get()
    {
        return Ok(new { Status = "Healthy" });
    }
}
