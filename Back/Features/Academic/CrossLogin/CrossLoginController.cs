using Syki.Back.Auth;

namespace Syki.Back.Features.Academic.CrossLogin;

/// <summary>
/// Realiza o login a partir do Acadêmico, indo para conta de um Aluno ou Professor da instituição.
/// </summary>
[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CrossLoginController(CrossLoginService service) : ControllerBase
{
    [Authorize(BackPolicy.CrossLogin)]
    [HttpPost("academic/cross-login")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Login([FromBody] CrossLoginIn data)
    {
        var result = await service.Login(User.InstitutionId(), data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
