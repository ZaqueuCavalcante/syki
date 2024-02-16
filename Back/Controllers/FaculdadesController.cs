using Syki.Shared;
using Syki.Back.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Authorization;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Back.Controllers;

[Authorize(Roles = Adm)]
[EnableRateLimiting("Medium")]
[ApiController, Route("[controller]")]
public class FaculdadesController : ControllerBase
{
    private readonly IFaculdadesService _service;
    public FaculdadesController(IFaculdadesService service) => _service = service;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] FaculdadeIn data)
    {
        var faculdade = await _service.Create(data);

        return Ok(faculdade);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var faculdades = await _service.GetAll();

        return Ok(faculdades);
    }
}
