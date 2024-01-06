using Syki.Shared;
using Syki.Back.Services;
using Syki.Back.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Back.Controllers;

[ApiController, Route("[controller]")]
public class OfertasController : ControllerBase
{
    private readonly IOfertasService _service;
    public OfertasController(IOfertasService service) => _service = service;

    [HttpPost("")]
    [Authorize(Roles = Academico)]
    public async Task<IActionResult> Create([FromBody] OfertaIn body)
    {
        var oferta = await _service.Create(User.Facul(), body);

        return Ok(oferta);
    }

    [HttpGet("")]
    [Authorize(Roles = Academico)]
    public async Task<IActionResult> GetAll()
    {
        var ofertas = await _service.GetAll(User.Facul());

        return Ok(ofertas);
    }
}
