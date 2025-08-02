namespace Syki.Back.Features.Adm.GetFeatureFlags;

[ApiController, AuthAdm]
public class GetFeatureFlagsController(GetFeatureFlagsService service) : ControllerBase
{
    /// <summary>
    /// Feature flags
    /// </summary>
    /// <remarks>
    /// Retorna as feature flags.
    /// </remarks>
    [HttpGet("adm/feature-flags")]
    public async Task<IActionResult> Get()
    {
        var featureFlags = await service.Get();

        return Ok(featureFlags);
    }
}
