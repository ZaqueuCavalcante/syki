using Syki.Back.Extensions;
using Syki.Shared.SetupMfa;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Authorization;

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
