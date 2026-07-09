namespace Estud.Back.Features.Adm.GetInstitutions;

[ApiController, Authorize]
public class GetInstitutionsController(GetInstitutionsService service) : ControllerBase
{
    /// <summary>
    /// Instituições
    /// </summary>
    /// <remarks>
    /// Retorna todas as instituições.
    /// </remarks>
    [HttpGet("adm/institutions")]
    public async Task<IActionResult> Get()
    {
        var institutions = await service.Get();

        return Ok(institutions);
    }
}
