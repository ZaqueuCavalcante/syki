namespace Syki.Back.Health;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
[Consumes("application/json"), Produces("application/json")]
public class HealthController : ControllerBase
{
    /// <summary>
    /// Retorna o Status da API.
    /// </summary>
    [HttpGet("health")]
    public IActionResult Get()
    {
        return Ok(new { Status = "Healthy" });
    }
}
