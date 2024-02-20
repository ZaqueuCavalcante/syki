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
public class CursosController : ControllerBase
{
    private readonly ICursosService _service;
    public CursosController(ICursosService service) => _service = service;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] CursoIn data)
    {
        var curso = await _service.Create(User.Facul(), data);

        return Ok(curso);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var cursos = await _service.GetAll(User.Facul());

        return Ok(cursos);
    }

    [HttpGet("disciplinas")]
    public async Task<IActionResult> GetAllWithDisciplinas()
    {
        var cursos = await _service.GetAllWithDisciplinas(User.Facul());

        return Ok(cursos);
    }

    [HttpGet("{id}/disciplinas")]
    public async Task<IActionResult> GetDisciplinas([FromRoute] Guid id)
    {
        var disciplinas = await _service.GetDisciplinas(id, User.Facul());

        return Ok(disciplinas);
    }
}
