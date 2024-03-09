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
