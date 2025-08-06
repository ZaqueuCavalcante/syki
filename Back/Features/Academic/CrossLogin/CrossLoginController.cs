using Syki.Back.Auth;

namespace Syki.Back.Features.Academic.CrossLogin;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class CrossLoginController(CrossLoginService service, AuthSettings settings) : ControllerBase
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
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Login([FromBody] CrossLoginIn data)
    {
        var result = await service.Login(User.InstitutionId(), data);

        if (result.IsSuccess())
        {
            Response.AppendSykiJwtCookie(result.GetSuccess().AccessToken, settings);
            result.GetSuccess().AccessToken = "";
        }

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CrossLoginIn>;
internal class ResponseExamples : ExamplesProvider<CrossLoginOut>;
internal class ErrorsExamples : ErrorExamplesProvider<UserNotFound>;
