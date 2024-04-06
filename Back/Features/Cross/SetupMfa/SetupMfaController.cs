namespace Syki.Back.Features.Cross.SetupMfa;

/// <summary>
/// Habilita a autenticação de dois fatores.
/// </summary>
[ApiController, AuthBearer]
[EnableRateLimiting("Small")]
[Consumes("application/json"), Produces("application/json")]
public class SetupMfaController(SetupMfaService service) : ControllerBase
{
    [HttpPost("mfa/setup")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Setup([FromBody] SetupMfaIn data)
    {
        await service.Setup(User.Id(), data.Token);

        return Ok();
    }
}