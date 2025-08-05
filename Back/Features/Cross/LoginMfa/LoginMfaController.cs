namespace Syki.Back.Features.Cross.LoginMfa;

[ApiController]
[EnableRateLimiting("Small")]
public class LoginMfaController(LoginMfaService service, AuthSettings settings) : ControllerBase
{
    /// <summary>
    /// Login MFA 🔓
    /// </summary>
    /// <remarks>
    /// Realiza login utilizando o token MFA.
    /// </remarks>
    [HttpPost("login/mfa")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> LoginMfa([FromBody] LoginMfaIn data)
    {
        var result = await service.LoginMfa(data);

        if (result.IsSuccess())
        {
            Response.AppendSykiJwtCookie(result.GetSuccess().AccessToken, settings);
        }

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<LoginMfaIn>;
internal class ResponseExamples : ExamplesProvider<LoginMfaOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    LoginRequiresTwoFactor,
    LoginWrongEmailOrPassword>;
