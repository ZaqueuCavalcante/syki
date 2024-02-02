using Syki.Shared;
using Syki.Back.Services;
using Syki.Back.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Back.Controllers;

[Authorize(Roles = Academico)]
[ApiController, Route("[controller]")]
public class GradesController : ControllerBase
{
    private readonly IGradesService _service;
    public GradesController(IGradesService service) => _service = service;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] GradeIn data)
    {
        var grade = await _service.Create(User.Facul(), data);

        return Ok(grade);
    }

    [HttpGet("{id}/disciplinas")]
    public async Task<IActionResult> GetDisciplinas([FromRoute] Guid id)
    {
        var grades = await _service.GetDisciplinas(User.Facul(), id);

        return Ok(grades);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var grades = await _service.GetAll(User.Facul());

        return Ok(grades);
    }
}
