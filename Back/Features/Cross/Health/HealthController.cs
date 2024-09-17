using System.Reflection;

namespace Syki.Back.Health;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
[Consumes("application/json"), Produces("application/json")]
public class HealthController : ControllerBase
{
    [HttpGet("health")]
    public IActionResult Get()
    {
        var informationalVersion = typeof(Program).Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            .InformationalVersion;

        Console.WriteLine(informationalVersion);

        return Ok(new { Status = "Healthy" });
    }
}
