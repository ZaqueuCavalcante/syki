using Syki.Shared;
using Syki.Back.Services;
using Syki.Back.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Back.Controllers;

[Authorize(Roles = Academico)]
[ApiController, Route("[controller]")]
public class CursosController : ControllerBase
{
    private readonly ICursosService _service;
    public CursosController(ICursosService service) => _service = service;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] CursoIn body)
    {
        var curso = await _service.Create(User.Facul(), body);

        return Ok(curso);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var cursos = await _service.GetAll(User.Facul());

        return Ok(cursos);
    }
}
