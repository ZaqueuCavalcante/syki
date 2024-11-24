namespace Syki.Back.Features.Cross.Login;

[ApiController]
[EnableRateLimiting("Small")]
[Consumes("application/json"), Produces("application/json")]
public class LoginController(LoginService service) : ControllerBase
{
    /// <summary>
    /// Login 🔓
    /// </summary>
    /// <remarks>
    /// Realiza login no sistema.
    /// </remarks>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginOut), 200)]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(200, typeof(LoginResponseExamples))]
    [SwaggerResponseExample(400, typeof(LoginErrorsExamples))]
    public async Task<IActionResult> Login([FromBody] LoginIn data)
    {
        var result = await service.Login(data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
