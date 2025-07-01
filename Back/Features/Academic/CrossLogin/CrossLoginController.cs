using Syki.Back.Auth;

namespace Syki.Back.Features.Academic.CrossLogin;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CrossLoginController(CrossLoginService service) : ControllerBase
{
    /// <summary>
    /// Login interno
    /// </summary>
    /// <remarks>
    /// Realiza o login a partir do Acadêmico, indo para conta de um Aluno ou Professor da instituição. <br/>
    /// Essa operação pode ser ativada/desativada globalmente pelo usuário Adm através da feature flag CrossLogin.
    /// </remarks>
    [Authorize(BackPolicies.CrossLogin)]
    [HttpPost("academic/cross-login")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Login([FromBody] CrossLoginIn data)
    {
        var result = await service.Login(User.InstitutionId(), data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
