namespace Syki.Back.ResetPassword;

[ApiController]
[EnableRateLimiting("Small")]
public class ResetPasswordController(ResetPasswordService service) : ControllerBase
{
    [HttpPost("reset-password")]
    public async Task<IActionResult> Reset([FromBody] ResetPasswordIn data)
    {
        await service.Reset(data);

        return Ok();
    }
}
