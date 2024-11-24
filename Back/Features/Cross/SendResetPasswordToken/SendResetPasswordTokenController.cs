namespace Syki.Back.Features.Cross.SendResetPasswordToken;

[ApiController]
[EnableRateLimiting("Small")]
[Consumes("application/json"), Produces("application/json")]
public class SendResetPasswordTokenController(SendResetPasswordTokenService service) : ControllerBase
{
    /// <summary>
    /// Esqueci minha senha 🔓
    /// </summary>
    /// <remarks>
    /// Envia para o email informado um link de redefinição de senha.
    /// </remarks>
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
