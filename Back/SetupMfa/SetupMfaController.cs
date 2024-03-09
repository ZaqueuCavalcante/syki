namespace Syki.Back.SetupMfa;

[ApiController, Authorize]
[EnableRateLimiting("Small")]
public class SetupMfaController(SetupMfaService service) : ControllerBase
{
    [HttpPost("mfa/setup")]
    public async Task<IActionResult> Setup([FromBody] SetupMfaIn data)
    {
        await service.Setup(User.Id(), data.Token);

        return Ok();
    }
}
