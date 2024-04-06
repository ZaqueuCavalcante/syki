namespace Syki.Back.Features.Cross.GetMfaKey;

/// <summary>
/// Retorna a chave MFA do usuário.
/// Ela é mostrada em tela via QR-Code.
/// </summary>
[ApiController, AuthBearer]
[EnableRateLimiting("Small")]
[Consumes("application/json"), Produces("application/json")]
public class GetMfaKeyController(GetMfaKeyService service) : ControllerBase
{
    [HttpGet("mfa/key")]
    [ProducesResponseType(200)]
    [SwaggerResponseExample(200, typeof(GetMfaKeyResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var key = await service.Get(User.Id());

        return Ok(key);
    }
}
