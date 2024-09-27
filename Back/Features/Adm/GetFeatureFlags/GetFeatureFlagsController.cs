namespace Syki.Back.Features.Adm.GetFeatureFlags;

/// <summary>
/// Retorna as Feature Flags.
/// </summary>
[ApiController, AuthAdm]
[Consumes("application/json"), Produces("application/json")]
public class GetFeatureFlagsController(GetFeatureFlagsService service) : ControllerBase
{
    [HttpGet("adm/feature-flags")]
    public async Task<IActionResult> Get()
    {
        var featureFlags = await service.Get();

        return Ok(featureFlags);
    }
}
