using System.Reflection;

namespace Exato.Back.Features.Cross.Health;

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
        var version = Assembly.GetEntryAssembly()?
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion;

        return Ok(new { Version = version });
    }
}
