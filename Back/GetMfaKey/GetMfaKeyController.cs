namespace Syki.Back.GetMfaKey;

[ApiController, Authorize]
[EnableRateLimiting("Small")]
public class GetMfaKeyController(GetMfaKeyService service) : ControllerBase
{
    [HttpGet("mfa/key")]
    public async Task<IActionResult> Get()
    {
        var key = await service.Get(User.Id());

        return Ok(key);
    }
}
