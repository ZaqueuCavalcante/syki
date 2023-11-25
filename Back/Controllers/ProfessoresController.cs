using Syki.Shared;
using Syki.Back.Services;
using Syki.Back.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Back.Controllers;

[Authorize(Roles = Academico)]
[ApiController, Route("[controller]")]
public class ProfessoresController : ControllerBase
{
    private readonly IProfessoresService _service;
    public ProfessoresController(IProfessoresService service) => _service = service;

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] ProfessorIn body)
    {
        var professor = await _service.Create(User.Facul(), body);

        return Ok(professor);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var professores = await _service.GetAll(User.Facul());

        return Ok(professores);
    }
}
