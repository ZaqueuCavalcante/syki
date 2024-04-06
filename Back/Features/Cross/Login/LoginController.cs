namespace Syki.Back.Features.Cross.Login;

/// <summary>
/// Realiza o login no sistema.
/// </summary>
[ApiController]
[EnableRateLimiting("Small")]
[Consumes("application/json"), Produces("application/json")]
public class LoginController(LoginService service) : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Login([FromBody] LoginIn data)
    {
        var result = await service.Login(data);

        return Ok(result);
    }
}
