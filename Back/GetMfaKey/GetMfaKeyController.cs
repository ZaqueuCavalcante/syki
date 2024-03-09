namespace Syki.Back.GetMfaKey;

[ApiController]
[EnableRateLimiting("Small")]
public class GetMfaKeyController(GetMfaKeyService service) : ControllerBase
{
    [AuthBearer]
    [HttpGet("mfa/key")]
    public async Task<IActionResult> Get()
    {
        var key = await service.Get(User.Id());

        return Ok(key);
    }
}
