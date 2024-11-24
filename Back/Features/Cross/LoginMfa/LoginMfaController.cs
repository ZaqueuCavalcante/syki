namespace Syki.Back.Features.Cross.LoginMfa;

[ApiController]
[EnableRateLimiting("Small")]
[Consumes("application/json"), Produces("application/json")]
public class LoginMfaController(LoginMfaService service) : ControllerBase
{
    /// <summary>
    /// Login MFA 🔓
    /// </summary>
    /// <remarks>
    /// Realiza login utilizando o token MFA.
    /// </remarks>
    [HttpPost("login/mfa")]
    [ProducesResponseType(typeof(LoginMfaOut), 200)]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(200, typeof(LoginMfaResponseExamples))]
    [SwaggerResponseExample(400, typeof(LoginMfaErrorsExamples))]
    public async Task<IActionResult> LoginMfa([FromBody] LoginMfaIn data)
    {
        var result = await service.LoginMfa(data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
