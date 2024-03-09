namespace Syki.Back.Controllers;

[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
public class OfertasController(IOfertasService service) : ControllerBase
{
    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] OfertaIn data)
    {
        var oferta = await service.Create(User.InstitutionId(), data);

        return Ok(oferta);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var ofertas = await service.GetAll(User.InstitutionId());

        return Ok(ofertas);
    }
}
