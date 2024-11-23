namespace Syki.Back.Features.Cross.GetMfaKey;

[ApiController, AuthBearer]
[EnableRateLimiting("Small")]
[Consumes("application/json"), Produces("application/json")]
public class GetMfaKeyController(GetMfaKeyService service) : ControllerBase
{
    /// <summary>
    /// Chave MFA
    /// </summary>
    /// <remarks>
    /// Ela é mostrada em tela via QR-Code.
    /// </remarks>
    [HttpGet("mfa/key")]
    [ProducesResponseType(200)]
    [SwaggerResponseExample(200, typeof(GetMfaKeyResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var key = await service.Get(User.Id());

        return Ok(key);
    }
}
