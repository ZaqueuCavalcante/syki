using Microsoft.AspNetCore.Mvc;
using Syki.Shared.CreatePendingDemo;
using Microsoft.AspNetCore.RateLimiting;

namespace Syki.Back.CreatePendingDemo;

[ApiController]
[EnableRateLimiting("VerySmall")]
public class CreatePendingDemoController : ControllerBase
{
    private readonly CreatePendingDemoService _service;
    public CreatePendingDemoController(CreatePendingDemoService service) => _service = service;

    [HttpPost("demos")]
    public async Task<IActionResult> Create([FromBody] CreatePendingDemoIn data)
    {
        await _service.Create(data);

        return Ok();
    }
}
