namespace Syki.Back.Controllers;

[EnableRateLimiting("Medium")]
[ApiController, Route("[controller]")]
public class OfertasController : ControllerBase
{
    private readonly IOfertasService _service;
    public OfertasController(IOfertasService service) => _service = service;

    [HttpPost("")]
    [AuthAcademico]
    public async Task<IActionResult> Create([FromBody] OfertaIn data)
    {
        var oferta = await _service.Create(User.Facul(), data);

        return Ok(oferta);
    }

    [HttpGet("")]
    [AuthAcademico]
    public async Task<IActionResult> GetAll()
    {
        var ofertas = await _service.GetAll(User.Facul());

        return Ok(ofertas);
    }
}
