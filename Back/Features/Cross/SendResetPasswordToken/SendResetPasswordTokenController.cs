namespace Syki.Back.Features.Cross.SendResetPasswordToken;

/// <summary>
/// Redefine a senha do Usuário.
/// </summary>
[ApiController]
[EnableRateLimiting("Small")]
[Consumes("application/json"), Produces("application/json")]
public class SendResetPasswordTokenController(SendResetPasswordTokenService service) : ControllerBase
{
    [HttpPost("reset-password-token")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(400, typeof(SendResetPasswordTokenErrorsExamples))]
    public async Task<IActionResult> Reset([FromBody] SendResetPasswordTokenIn data)
    {
        var result = await service.Send(data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
