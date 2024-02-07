using Syki.Shared;
using Syki.Back.Services;
using Syki.Back.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Back.Controllers;

[ApiController, Route("[controller]")]
public class MatriculasController : ControllerBase
{
    private readonly IMatriculasService _service;
    public MatriculasController(IMatriculasService service) => _service = service;

    [HttpPost("periodos")]
    [Authorize(Roles = Academico)]
    public async Task<IActionResult> CreatePeriodoDeMatricula([FromBody] PeriodoDeMatriculaIn data)
    {
        var periodo = await _service.CreatePeriodoDeMatricula(User.Facul(), data);

        return Ok(periodo);
    }

    [HttpGet("periodos")]
    [Authorize(Roles = Academico)]
    public async Task<IActionResult> GetPeriodosDeMatricula()
    {
        var periodos = await _service.GetPeriodosDeMatricula(User.Facul());

        return Ok(periodos);
    }

    [HttpGet("turmas")]
    [Authorize(Roles = Aluno)]
    public async Task<IActionResult> GetTurmas()
    {
        var turmas = await _service.GetTurmas(User.Facul(), User.Id());

        return Ok(turmas);
    }
}
