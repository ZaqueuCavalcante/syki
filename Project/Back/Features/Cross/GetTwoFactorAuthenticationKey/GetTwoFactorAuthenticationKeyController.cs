using Exato.Shared.Features.Cross.GetTwoFactorAuthenticationKey;

namespace Exato.Back.Features.Cross.GetTwoFactorAuthenticationKey;

[ApiController, Authorize(Policies.GetTwoFactorAuthenticationKey)]
public class GetTwoFactorAuthenticationKeyController(GetTwoFactorAuthenticationKeyService service) : ControllerBase
{
    /// <summary>
    /// Chave 2FA
    /// </summary>
    /// <remarks>
    /// Retorna a chave 2FA do usuário. <br/>
    /// Ela é mostrada em tela via QR-Code, possibilitando a configuração do 2FA.
    /// </remarks>
    [HttpGet("2fa/key")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var key = await service.Get(User.Id);

        return Ok(key);
    }
}

internal class ResponseExamples : ExamplesProvider<GetTwoFactorAuthenticationKeyOut>;
