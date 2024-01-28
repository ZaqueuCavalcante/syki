using Syki.Shared;
using Syki.Back.Services;
using Syki.Back.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Back.Controllers;

[ApiController, Route("[controller]")]
public class AlunosController : ControllerBase
{
    private readonly IAlunosService _service;
    public AlunosController(IAlunosService service) => _service = service;

    [HttpPost("")]
    [Authorize(Roles = Academico)]
    public async Task<IActionResult> Create([FromBody] AlunoIn data)
    {
        var aluno = await _service.Create(User.Facul(), data);

        return Ok(aluno);
    }

    [HttpGet("disciplinas")]
    [Authorize(Roles = Aluno)]
    public async Task<IActionResult> GetDisciplinas()
    {
        var disciplinas = await _service.GetDisciplinas(User.Id());

        return Ok(disciplinas);
    }

    [HttpGet("")]
    [Authorize(Roles = Academico)]
    public async Task<IActionResult> GetAll()
    {
        var alunos = await _service.GetAll(User.Facul());
        
        return Ok(alunos);
    }
}
