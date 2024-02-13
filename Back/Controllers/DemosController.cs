using Syki.Shared;
using Syki.Back.Services;
using Microsoft.AspNetCore.Mvc;

namespace Syki.Back.Controllers;

[ApiController, Route("[controller]")]
public class DemosController : ControllerBase
{
    private readonly IDemosService _service;
    public DemosController(IDemosService service) => _service = service;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] DemoIn data)
    {
        var demo = await _service.Create(data);

        return Ok(demo);
    }

    [HttpPost("setup")]
    public async Task<IActionResult> Setup([FromBody] DemoSetupIn data)
    {
        var ok = await _service.Setup(data);

        return Ok(ok);
    }
}
