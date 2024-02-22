using Syki.Shared;
using Syki.Back.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Syki.Back.Controllers;

[ApiController, Route("[controller]")]
public class DemosController : ControllerBase
{
    private readonly IDemosService _service;
    public DemosController(IDemosService service) => _service = service;

    [HttpPost("setup")]
    [EnableRateLimiting("Small")]
    public async Task<IActionResult> Setup([FromBody] DemoSetupIn data)
    {
        var ok = await _service.Setup(data);

        return Ok(ok);
    }
}
