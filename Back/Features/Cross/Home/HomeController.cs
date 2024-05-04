namespace Syki.Back.Home;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class HomeController : ControllerBase
{
    [HttpGet("")]
    public IActionResult Get() => Redirect("/docs");
}
