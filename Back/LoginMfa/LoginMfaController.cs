using Syki.Shared.LoginMfa;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Syki.Back.LoginMfa;

[ApiController]
[EnableRateLimiting("Small")]
public class LoginMfaController(LoginMfaService service) : ControllerBase
{
    [HttpPost("mfa/login")]
    public async Task<IActionResult> LoginMfa([FromBody] LoginMfaIn data)
    {
        var result = await service.Login(data);

        return Ok(result);
    }
}
