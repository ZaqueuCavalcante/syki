using Syki.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Syki.Back.Login;

[ApiController]
[EnableRateLimiting("Small")]
public class LoginController : ControllerBase
{
    private readonly LoginService _service;
    public LoginController(LoginService service) => _service = service;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginIn data)
    {
        var result = await _service.Login(data);

        return Ok(result);
    }
}
