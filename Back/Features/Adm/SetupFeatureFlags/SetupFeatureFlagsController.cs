namespace Syki.Back.Features.Adm.SetupFeatureFlags;

[ApiController, AuthAdm]
[Consumes("application/json"), Produces("application/json")]
public class SetupFeatureFlagsController(SetupFeatureFlagsService service) : ControllerBase
{
    [HttpPut("adm/feature-flags")]
    public async Task<IActionResult> Setup([FromBody] SetupFeatureFlagsIn data)
    {
        await service.Setup(data);

        return Ok();
    }
}
