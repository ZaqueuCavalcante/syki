using Microsoft.AspNetCore.Mvc;

namespace Syki.Mocks.Turnstile;

[ApiController]
[Route("turnstile")]
public class TurnstileController : ControllerBase
{
    [HttpPost("siteverify")]
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
