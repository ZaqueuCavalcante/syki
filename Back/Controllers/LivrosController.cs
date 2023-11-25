using Syki.Shared;
using Syki.Back.Services;
using Syki.Back.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Back.Controllers;

[ApiController, Route("[controller]")]
public class LivrosController : ControllerBase
{
    private readonly ILivrosService _service;
    public LivrosController(ILivrosService service) => _service = service;

    [HttpPost("")]
    [Authorize(Roles = Academico)]
    public async Task<IActionResult> Create([FromBody] LivroIn body)
    {
        var livro = await _service.Create(User.Facul(), body);

        return Ok(livro);
    }

    [HttpGet("")]
    [Authorize(Roles = Academico)]
    public async Task<IActionResult> GetAll()
    {
        var livros = await _service.GetAll(User.Facul());
        
        return Ok(livros);
    }
}
