namespace Syki.Back.SendResetPasswordToken;

[ApiController]
[EnableRateLimiting("Small")]
public class SendResetPasswordTokenController(SendResetPasswordEmailService service) : ControllerBase
{
    [HttpPost("reset-password-token")]
    public async Task<IActionResult> Reset([FromBody] SendResetPasswordTokenIn data)
    {
        await service.Send(data);

        return Ok();
    }
}
