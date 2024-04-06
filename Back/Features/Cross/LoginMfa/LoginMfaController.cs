namespace Syki.Back.Features.Cross.LoginMfa;

/// <summary>
/// Realiza o login utilizando o token de segundo fator de autenticação.
/// </summary>
[ApiController]
[EnableRateLimiting("Small")]
[Consumes("application/json"), Produces("application/json")]
public class LoginMfaController(LoginMfaService service) : ControllerBase
{
    [HttpPost("login/mfa")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> LoginMfa([FromBody] LoginMfaIn data)
    {
        var result = await service.Login(data);

        return Ok(result);
    }
}
