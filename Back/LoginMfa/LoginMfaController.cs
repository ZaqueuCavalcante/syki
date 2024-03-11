namespace Syki.Back.LoginMfa;

[ApiController]
[EnableRateLimiting("Small")]
public class LoginMfaController(LoginMfaService service) : ControllerBase
{
    [HttpPost("login/mfa")]
    public async Task<IActionResult> LoginMfa([FromBody] LoginMfaIn data)
    {
        var result = await service.Login(data);

        return Ok(result);
    }
}
