namespace Syki.Back.Features.Identity.GetTwoFactorKey;

[ApiController, Authorize(Policies.GetTwoFactorKey)]
public class GetTwoFactorKeyController(GetTwoFactorKeyService service) : ControllerBase
{
    /// <summary>
    /// Chave 2FA
    /// </summary>
    /// <remarks>
    /// Retorna a chave 2FA do usuário.
    /// </remarks>
    [HttpGet("identity/2fa-key")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var key = await service.Get();
        return Ok(key);
    }
}

internal class ResponseExamples : ExamplesProvider<GetTwoFactorKeyOut>;
