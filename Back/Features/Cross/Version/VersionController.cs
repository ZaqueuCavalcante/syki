namespace Syki.Back.Features.Cross.Version;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class VersionController : ControllerBase
{
    /// <summary>
    /// Retorna a versão atual do código.
    /// </summary>
    [HttpGet("version")]
    public IActionResult Get()
    {
        return Ok(new { Version = AppVersion.Value });
    }
}
