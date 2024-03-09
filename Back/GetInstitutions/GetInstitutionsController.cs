namespace Syki.Back.GetInstitutions;

[ApiController, AuthAdm]
public class GetInstitutionsController(GetInstitutionsService service) : ControllerBase
{
    [HttpGet("institutions")]
    public async Task<IActionResult> Get()
    {
        var institutions = await service.Get();

        return Ok(institutions);
    }
}
