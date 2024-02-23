using Microsoft.AspNetCore.Mvc;
using Syki.Shared.CreatePendingDemo;
using Microsoft.AspNetCore.RateLimiting;

namespace Syki.Back.CreatePendingDemo;

[ApiController]
[EnableRateLimiting("VerySmall")]
public class CreatePendingDemoController(CreatePendingDemoService service) : ControllerBase
{
    [HttpPost("demos")]
    public async Task<IActionResult> Create([FromBody] CreatePendingDemoIn data)
    {
        await service.Create(data);

        return Ok();
    }
}
