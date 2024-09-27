namespace Syki.Back.Features.Adm.GetInstitutions;

/// <summary>
/// Retorna todas as Instituições.
/// </summary>
[ApiController, AuthAdm]
[Consumes("application/json"), Produces("application/json")]
public class GetInstitutionsController(GetInstitutionsService service) : ControllerBase
{
    [HttpGet("adm/institutions")]
    public async Task<IActionResult> Get()
    {
        var institutions = await service.Get();

        return Ok(institutions);
    }
}
