namespace Syki.Back.CreateOferta;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class CreateOfertaController(CreateOfertaService service) : ControllerBase
{
    [HttpPost("ofertas")]
    public async Task<IActionResult> Create([FromBody] OfertaIn data)
    {
        var oferta = await service.Create(User.InstitutionId(), data);

        return Ok(oferta);
    }
}
