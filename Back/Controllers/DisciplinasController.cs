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
public class DisciplinasController : ControllerBase
{
    private readonly IDisciplinasService _service;
    public DisciplinasController(IDisciplinasService service) => _service = service;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] DisciplinaIn data)
    {
        var disciplina = await _service.Create(User.Facul(), data);

        return Ok(disciplina);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] Guid? cursoId)
    {
        var disciplinas = await _service.GetAll(User.Facul(), cursoId);

        return Ok(disciplinas);
    }
}
