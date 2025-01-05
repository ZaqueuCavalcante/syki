namespace Syki.Back.Home;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
[Consumes("application/json"), Produces("application/json")]
public class HomeController : ControllerBase
{
    /// <summary>
    /// Redireciona para a Documentação da API.
    /// </summary>
    [HttpGet("")]
    public IActionResult Get()
    {
        return Redirect("/docs/v1");
    }
}
