using Microsoft.AspNetCore.Mvc;

namespace Syki.Back.Controllers;

[ApiController, Route("")]
[ApiExplorerSettings(IgnoreApi = true)]
public class HomeController : ControllerBase
{
    [HttpGet("")]
    public IActionResult Get() => Redirect("/swagger");
}
