using Syki.Back.Auth;

namespace Syki.Back.Features.Cross.SkipUserRegister;

/// <summary>
/// Loga direto no sistema, pulando a fase de Cadastro.
/// Endpoint habilitado apenas em momentos pontuais para demonstração.
/// </summary>
[ApiController]
[EnableRateLimiting("VerySmall")]
[Consumes("application/json"), Produces("application/json")]
public class SkipUserRegisterController(SkipUserRegisterService service) : ControllerBase
{
    [HttpPost("skip-user-register")]
    [Authorize(BackPolicy.SkipUserRegister)]
    public async Task<IActionResult> Skip([FromBody] SkipUserRegisterLoginIn data)
    {
        var result = await service.Skip(data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
