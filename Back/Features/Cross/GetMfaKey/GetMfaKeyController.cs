namespace Syki.Back.Features.Cross.GetMfaKey;

[ApiController, AuthBearer]
[EnableRateLimiting("Small")]
public class GetMfaKeyController(GetMfaKeyService service) : ControllerBase
{
    /// <summary>
    /// Chave MFA
    /// </summary>
    /// <remarks>
    /// Retorna a chave MFA do usuário. <br/>
    /// Ela é mostrada em tela via QR-Code, possibilitando a configuração do MFA.
    /// </remarks>
    [HttpGet("mfa/key")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var key = await service.Get(User.Id);

        return Ok(key);
    }
}

internal class ResponseExamples : ExamplesProvider<GetMfaKeyOut>;
