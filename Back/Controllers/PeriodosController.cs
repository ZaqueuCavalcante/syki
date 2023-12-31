using Syki.Shared;
using Syki.Back.Services;
using Syki.Back.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Back.Controllers;

[Authorize(Roles = Academico)]
[ApiController, Route("[controller]")]
public class PeriodosController : ControllerBase
{
    private readonly IPeriodosService _service;
    public PeriodosController(IPeriodosService service) => _service = service;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] PeriodoIn body)
    {
        var periodo = await _service.Create(User.Facul(), body);

        return Ok(periodo);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var periodos = await _service.GetAll(User.Facul());

        return Ok(periodos.ConvertAll(p => p.Id));
    }
}
