namespace Syki.Back.GetOfertas;

[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
public class GetOfertasController(GetOfertasService service) : ControllerBase
{
    [HttpGet("ofertas")]
    public async Task<IActionResult> Get()
    {
        var ofertas = await service.Get(User.InstitutionId());

        return Ok(ofertas);
    }
}
