namespace Syki.Back.SetupMfa;

[ApiController]
[EnableRateLimiting("Small")]
public class SetupMfaController(SetupMfaService service) : ControllerBase
{
    [AuthBearer]
    [HttpPost("mfa/setup")]
    public async Task<IActionResult> Setup([FromBody] SetupMfaIn data)
    {
        await service.Setup(User.Id(), data.Token);

        return Ok();
    }
}
