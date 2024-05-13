namespace Syki.Back.Features.Adm.GetInstitutions;

[ApiController, AuthAdm]
public class GetInstitutionsController(GetInstitutionsService service) : ControllerBase
{
    [HttpGet("adm/institutions")]
    public async Task<IActionResult> Get()
    {
        var institutions = await service.Get();

        return Ok(institutions);
    }
}
