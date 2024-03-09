namespace Syki.Back.CreateInstitution;

[ApiController, AuthAdm]
[EnableRateLimiting("Medium")]
public class CreateInstitutionController(CreateInstitutionService service) : ControllerBase
{
    [HttpPost("institutions")]
    public async Task<IActionResult> Create([FromBody] FaculdadeIn data)
    {
        var faculdade = await service.Create(data);

        return Ok(faculdade);
    }
}
