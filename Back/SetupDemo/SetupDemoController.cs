using Syki.Shared.SetupDemo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Syki.Back.SetupDemo;

[ApiController]
[EnableRateLimiting("Small")]
public class SetupDemoController : ControllerBase
{
    private readonly SetupDemoService _service;
    public SetupDemoController(SetupDemoService service) => _service = service;

    [HttpPost("demos/setup")]
    public async Task<IActionResult> Setup([FromBody] SetupDemoIn data)
    {
        await _service.Setup(data);

        return Ok();
    }
}
