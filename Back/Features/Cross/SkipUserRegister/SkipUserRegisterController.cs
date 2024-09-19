using Syki.Back.Auth;

namespace Syki.Back.Features.Cross.SkipUserRegister;

[ApiController]
[EnableRateLimiting("VerySmall")]
[Consumes("application/json"), Produces("application/json")]
public class SkipUserRegisterController(SkipUserRegisterService service) : ControllerBase
{
    [HttpPost("skip-user-register")]
    [Authorize(BackPolicy.SkipUserRegister)]
    public async Task<IActionResult> Skip([FromBody] SkipUserRegisterLoginIn data)
    {
        var result = await service.Skip(data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
