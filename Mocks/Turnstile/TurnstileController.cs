using Microsoft.AspNetCore.Mvc;

namespace Estud.Mocks.Turnstile;

[ApiController]
public class TurnstileController : ControllerBase
{
    [HttpPost("turnstile/siteverify")]
    public IActionResult SiteVerify([FromForm] string secret, [FromForm] string response)
    {
        var success = response != "INVALID";
        return Ok(new
        {
            success,
            error_codes = success ? Array.Empty<string>() : ["invalid-input-response"]
        });
    }
}
