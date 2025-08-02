namespace Syki.Back.Features.Adm.SetupFeatureFlags;

[ApiController, AuthAdm]
public class SetupFeatureFlagsController(SetupFeatureFlagsService service) : ControllerBase
{
    /// <summary>
    /// Editar feature flags
    /// </summary>
    /// <remarks>
    /// Define novos valores para as feature flags.
    /// </remarks>
    [HttpPut("adm/feature-flags")]
    public async Task<IActionResult> Setup([FromBody] SetupFeatureFlagsIn data)
    {
        await service.Setup(data);

        return Ok();
    }
}
