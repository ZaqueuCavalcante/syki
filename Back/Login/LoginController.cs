using Syki.Shared.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Syki.Back.Login;

[ApiController]
[EnableRateLimiting("Small")]
public class LoginController(LoginService service) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginIn data)
    {
        var result = await service.Login(data);

        return Ok(result);
    }
}
