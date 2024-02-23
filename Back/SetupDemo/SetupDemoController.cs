using Syki.Shared.SetupDemo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Syki.Back.SetupDemo;

[ApiController]
[EnableRateLimiting("Small")]
public class SetupDemoController(SetupDemoService service) : ControllerBase
{
    [HttpPost("demos/setup")]
    public async Task<IActionResult> Setup([FromBody] SetupDemoIn data)
    {
        await service.Setup(data);

        return Ok();
    }
}
