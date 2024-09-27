namespace Syki.Back.Home;

/// <summary>
/// Redireciona para a Documentação da API.
/// </summary>
[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
[Consumes("application/json"), Produces("application/json")]
public class HomeController : ControllerBase
{
    [HttpGet("")]
    public IActionResult Get() => Redirect("/docs");
}
