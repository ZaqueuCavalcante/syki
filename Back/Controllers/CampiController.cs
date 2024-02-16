using Syki.Shared;
using Syki.Back.Services;
using Syki.Back.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Authorization;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Back.Controllers;

[Authorize(Roles = Academico)]
[EnableRateLimiting("Medium")]
[ApiController, Route("[controller]")]
public class CampiController : ControllerBase
{
    private readonly ICampiService _service;
    public CampiController(ICampiService service) => _service = service;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] CampusIn data)
    {
        var campus = await _service.Create(User.Facul(), data);

        return Ok(campus);
    }

    [HttpPut("")]
    public async Task<IActionResult> Update([FromBody] CampusUp data)
    {
        var campus = await _service.Update(User.Facul(), data);

        return Ok(campus);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var campi = await _service.GetAll(User.Facul());

        return Ok(campi);
    }
}
