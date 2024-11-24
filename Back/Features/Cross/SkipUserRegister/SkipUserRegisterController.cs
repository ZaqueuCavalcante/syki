using Syki.Back.Auth;

namespace Syki.Back.Features.Cross.SkipUserRegister;

[ApiController]
[EnableRateLimiting("VerySmall")]
[Consumes("application/json"), Produces("application/json")]
public class SkipUserRegisterController(SkipUserRegisterService service) : ControllerBase
{
    /// <summary>
    /// Pular registro
    /// </summary>
    /// <remarks>
    /// Loga direto no sistema, pulando a fase de cadastro. <br/>
    /// Endpoint habilitado apenas em momentos pontuais para demonstração. <br/>
    /// Essa operação pode ser ativada/desativada globalmente pelo usuário Adm através da feature flag SkipUserRegister.
    /// </remarks>
    [HttpPost("skip-user-register")]
    [Authorize(BackPolicy.SkipUserRegister)]
    public async Task<IActionResult> Skip([FromBody] SkipUserRegisterLoginIn data)
    {
        var result = await service.Skip(data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
